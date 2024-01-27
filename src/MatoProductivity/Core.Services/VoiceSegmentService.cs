using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Threading.Timers;
using Baidu.Aip.Speech;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Infrastructure.Common;
using MatoProductivity.ViewModels;
using Microsoft.Maui.Dispatching;
using Newtonsoft.Json;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        public Command TranslatedContent { get; set; }

        AsyncAudioPlayer audioPlayer;
        IAudioRecorder audioRecorder;
        readonly Stopwatch recordingStopwatch = new Stopwatch();
        readonly Stopwatch playingStopwatch = new Stopwatch();
        private readonly IAudioManager audioManager;
        private readonly AbpAsyncTimer timer;

        public VoiceSegmentService(
            AbpAsyncTimer timer,
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += VoiceSegmentViewModel_PropertyChanged;
            this.RemoveAudio = new Command(RemoveAudioAction);
            this.PlayAudio = new Command(PlayAudioAction, () => !IsPlaying&& IsFileContentNotEmpty);
            this.StopRecordAudio = new Command(StopRecordAudioAction, () => IsRecording);
            this.StopPlayAudio = new Command(StopPlayAudioAction, () => IsPlaying);
            this.RecordAudio = new Command(RecordAudioAction, () => !IsRecording);
            this.TranslatedContent = new Command(TranslatedContentAction, (o) => !HasTranslatedContent && IsFileContentNotEmpty);

            this.audioManager = AudioManager.Current;
            this.timer = timer;
            this.timer.Period = 500;
            this.timer.Elapsed = async (timer) =>
            {
                await Task.Run(() => RaisePropertyChanged(nameof(RecordingTime)));
                await Task.Run(() => RaisePropertyChanged(nameof(PlayingTime)));
            };
            this.timer.Start();
        }

        public TimeSpan RecordingTime
        {
            get => TimeSpan.FromMilliseconds(recordingStopwatch.ElapsedMilliseconds);
        }

        public TimeSpan PlayingTime
        {
            get => TimeSpan.FromMilliseconds(playingStopwatch.ElapsedMilliseconds);
        }

        private bool isPlaying;
        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                RaisePropertyChanged();
                PlayAudio.ChangeCanExecute();
                StopPlayAudio.ChangeCanExecute();
            }
        }

        private bool hasTranslatedContent;
        public bool HasTranslatedContent
        {
            get => hasTranslatedContent;
            set
            {
                hasTranslatedContent = value;
                RaisePropertyChanged();
                TranslatedContent.ChangeCanExecute();
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
                this.HasTranslatedContent=false;
                TranslatedContent.ChangeCanExecute();
                PlayAudio.ChangeCanExecute();
            }
        }

        private async void TranslatedContentAction(object obj)
        {
            this.Loading=true;

            var asrClient = new Asr("11797113", "UsXraVVnzHbzwXGuCD0Z4d9b", "8kGEszPmYxN5WGEHYV8yzx80zGVZElcX");
            asrClient.Timeout = 120000;
            var translated = await Task.Run(() =>
            {
                var result = asrClient.Recognize(this.FileContent, "pcm", 16000);
                var msg = result["result"]?.ToString();
                return msg;
            });
            Console.WriteLine(translated);
            if (string.IsNullOrEmpty(translated))
            {
                this.Loading=false;
                return;
            }
            var translatedObject = JsonConvert.DeserializeObject<List<string>>(translated);
            var translatedContents = string.Join('\n', translatedObject);

            if (Container is EditNotePageViewModel)
            {

                var note = new NoteSegment()
                {
                    Title = this.NoteSegment.Title+" - 识别结果",
                    Type= "TextSegment",
                    Icon=FaIcons.IconStickyNoteO,
                    NoteSegmentPayloads= new List<NoteSegmentPayload>()
                };
                note.NoteSegmentPayloads.Add(new NoteSegmentPayload("Content", translatedContents));
                (Container as EditNotePageViewModel).CreateSegment.Execute(note);
                this.HasTranslatedContent=true;
                this.Loading=false;

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
                playingStopwatch.Restart();

                await audioPlayer.PlayAsync(CancellationToken.None);

                IsPlaying = false;
                playingStopwatch.Stop();
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



    }
}
