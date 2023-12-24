using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using Microsoft.Maui.Dispatching;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;
using AudioManager = Plugin.Maui.Audio.AudioManager;

namespace MatoProductivity.Core.Services
{
    public class VoiceSegmentService : FileSegmentService, ITransientDependency
    {
        public Command RemoveAudio { get; set; }
        public Command PlayAudio { get; set; }
        public Command StopRecordAudio { get; set; }
        public Command StopPlayAudio { get; set; }
        public Command RecordAudio { get; set; }

        AsyncAudioPlayer audioPlayer;
        IAudioRecorder audioRecorder;
        readonly Stopwatch recordingStopwatch = new Stopwatch();
        private readonly IAudioManager audioManager;
        private readonly IDispatcher dispatcher;

        public VoiceSegmentService(
            IDispatcher dispatcher,
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            INoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += VoiceSegmentViewModel_PropertyChanged;
            this.RemoveAudio = new Command(RemoveAudioAction);
            this.PlayAudio = new Command(PlayAudioAction, () => !IsPlaying);
            this.StopRecordAudio = new Command(StopRecordAudioAction, () => IsRecording);
            this.StopPlayAudio = new Command(StopPlayAudioAction, () => IsPlaying);
            this.RecordAudio = new Command(RecordAudioAction, () => !IsRecording);

            this.audioManager = AudioManager.Current;
            this.dispatcher = dispatcher;
        }

        public double RecordingTime
        {
            get => recordingStopwatch.ElapsedMilliseconds / 1000;
        }

        private bool isPlaying;
        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                PlayAudio.ChangeCanExecute();
                StopPlayAudio.ChangeCanExecute();
            }
        }

        public bool IsRecording
        {
            get => audioRecorder?.IsRecording ?? false;
        }

        private void RemoveAudioAction(object obj)
        {
            throw new NotImplementedException();
        }

        private void VoiceSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(FileContent))
            {

            }


        }


        async void PlayAudioAction()
        {
            if (this.FileContent != null)
            {
                using (var stream = new MemoryStream(FileContent))
                {
                    audioPlayer = this.audioManager.CreateAsyncPlayer(stream);
                }
                IsPlaying = true;

                await audioPlayer.PlayAsync(CancellationToken.None);

                IsPlaying = false;
            }
        }

        void StopPlayAudioAction()
        {
            audioPlayer.Stop();
        }

        async void RecordAudioAction()
        {
            if (await CheckPermissionIsGrantedAsync<Microphone>())
            {
                audioRecorder = audioManager.CreateRecorder();

                await audioRecorder.StartAsync();
            }

            recordingStopwatch.Restart();
            UpdateRecordingTime();
            RaisePropertyChanged(nameof(IsRecording));
            RecordAudio.ChangeCanExecute();
            StopRecordAudio.ChangeCanExecute();
        }

        async void StopRecordAudioAction()
        {
            var audioSource = await audioRecorder.StopAsync();
            using (var stream = audioSource.GetAudioStream())
            using (MemoryStream fileStream = new MemoryStream())
            {
                await stream.CopyToAsync(fileStream);
                this.FileContent = fileStream.ToArray();
            }
            recordingStopwatch.Stop();
            RaisePropertyChanged(nameof(IsRecording));
            RecordAudio.ChangeCanExecute();
            StopRecordAudio.ChangeCanExecute();
        }

        void UpdateRecordingTime()
        {
            if (IsRecording is false)
            {
                return;
            }

            dispatcher.DispatchDelayed(
                TimeSpan.FromMilliseconds(16),
                () =>
                {
                    RaisePropertyChanged(nameof(RecordingTime));

                    UpdateRecordingTime();
                });
        }


    }
}
