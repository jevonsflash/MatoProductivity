﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using MatoProductivity.Core.Helper;
using MatoProductivity.Core.Interfaces;
using MatoProductivity.Core.Settings;
using MatoProductivity.Core.ViewModel;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Core.Services
{
    public class MusicRelatedService : ViewModelBase, ISingletonDependency
    {
        public event EventHandler OnBuildMusicInfosFinished;
        private readonly IMusicControlService musicControlService;
        private bool IsInitFinished = false;
        private bool _isInited = false;

        public MusicRelatedService(IMusicControlService musicControlService)
        {
            this.musicControlService = musicControlService;
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), DoUpdate);
            this.PropertyChanged+=MusicRelatedService_PropertyChangedAsync;

        }

        private async void MusicRelatedService_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentMusic))
            {
                if (!Canplay || IsInited == false)
                {
                    return;

                }
                await musicControlService.InitPlayer(CurrentMusic);
                DoUpdate();
                InitPreviewAndNextMusic();
                Duration = GetPlatformSpecificTime(musicControlService.Duration());
                SettingManager.ChangeSettingForApplication(CommonSettingNames.BreakPointMusicIndex, Musics.IndexOf(CurrentMusic).ToString());

            }

            else if (e.PropertyName == nameof(IsShuffle))
            {
                if (IsShuffle)
                {
                    await musicControlService.UpdateShuffleMap();
                    InitPreviewAndNextMusic();
                }
                else
                {
                    InitPreviewAndNextMusic();
                }
            }

            else if (e.PropertyName == nameof(IsRepeatOne))
            {
                musicControlService.SetRepeatOneStatus(this.IsRepeatOne);
            }
        }

        private MusicInfo _currentMusic;

        /// <summary>
        /// 当前曲目
        /// </summary>
        public MusicInfo CurrentMusic
        {
            get
            {
                return _currentMusic;
            }
            set
            {
                _currentMusic = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Canplay));
            }
        }


        private List<MusicInfo> _musics;

        /// <summary>
        /// 当前播放列表
        /// </summary>
        public List<MusicInfo> Musics
        {
            get
            {
                _musics = musicControlService.MusicInfos;
                return _musics;
            }
            set
            {
                _musics = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanplayAll));
            }
        }


        /// <summary>
        /// 是否可播放
        /// </summary>
        public bool Canplay => this.CurrentMusic != null;
        public bool CanplayAll => Musics.Count > 0;


        private bool _isPlaying;

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;

                    RaisePropertyChanged();

                }
            }
        }



        private bool _isShuffle;

        /// <summary>
        /// 是否随机播放
        /// </summary>
        public bool IsShuffle
        {
            get
            {
                if (!IsInitFinished)
                {
                    this._isShuffle = bool.Parse(this.SettingManager.GetSettingValue(CommonSettingNames.IsShuffle));
                    IsInitFinished = true;
                }

                return _isShuffle;
            }
            set
            {
                if (value != _isShuffle)
                {
                    _isShuffle = value;
                    RaisePropertyChanged();

                }
            }
        }

        /// <summary>
        /// 是否单曲循环
        /// </summary>
        private bool _isRepeatOne;

        public bool IsRepeatOne
        {
            get
            {
                if (!IsInitFinished)
                {
                    this._isRepeatOne = bool.Parse(this.SettingManager.GetSettingValue(CommonSettingNames.IsRepeatOne));
                    IsInitFinished = true;
                }
                return _isRepeatOne;
            }
            set
            {
                if (value != _isRepeatOne)
                {
                    _isRepeatOne = value;
                    RaisePropertyChanged();

                }
            }
        }

        private double _duration;

        /// <summary>
        /// 当前曲目总时间
        /// </summary>
        public double Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;

                RaisePropertyChanged();
            }
        }

        private double _currentTime;

        /// <summary>
        /// 当前进度
        /// </summary>
        public double CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                RaisePropertyChanged();
            }
        }


        private MusicInfo _previewMusic;

        /// <summary>
        /// 上一曲目
        /// </summary>
        public MusicInfo PreviewMusic
        {
            get
            {
                return _previewMusic;
            }
            set
            {
                _previewMusic = value;
                RaisePropertyChanged();
            }
        }
        private MusicInfo _nextMusic;

        /// <summary>
        /// 下一曲目
        /// </summary>
        public MusicInfo NextMusic
        {
            get
            {
                return _nextMusic;
            }
            set
            {
                _nextMusic = value;
                RaisePropertyChanged();
            }
        }



        public async Task BuildMusicInfos()
        {
            await musicControlService.RebuildMusicInfos(null).ContinueWith((e) =>
            {
                this.OnBuildMusicInfosFinished?.Invoke(this, EventArgs.Empty);
            });
        }

        public async Task InitAll()
        {
            OnBuildMusicInfosFinished+=(o, e) =>
            {
                //当队列初始化完成时初始化当前曲目
                CommonHelper.BeginInvokeOnMainThread(() =>
                {
                    InitCurrentMusic();
                });
                musicControlService.SetRepeatOneStatus(IsRepeatOne);
                musicControlService.OnPlayStatusChanged += MusicControlService_OnPlayStatusChanged;
                this._isInited = true;
            };

            await BuildMusicInfos();
        }


        public bool IsInited => _isInited;
        public bool GetIsInited()
        {
            return this._isInited;
        }

        /// <summary>
        /// 初始化下一首/上一首曲目
        /// </summary>
        public void InitPreviewAndNextMusic()
        {
            this.PreviewMusic = musicControlService.GetPreMusic(this.CurrentMusic, IsShuffle);
            this.NextMusic = musicControlService.GetNextMusic(this.CurrentMusic, IsShuffle);
        }

        public void InitCurrentMusic()
        {
            var musicIndex = int.Parse(this.SettingManager.GetSettingValue(CommonSettingNames.BreakPointMusicIndex));
            if (Musics.Count > 0)
            {
                if (musicIndex >= 0 && musicIndex <= Musics.Count - 1)
                {
                    CurrentMusic = Musics[musicIndex];
                }
                else
                {
                    CurrentMusic = Musics[0];
                }
                musicControlService.InitPlayer(CurrentMusic);

                this.Duration = GetPlatformSpecificTime(musicControlService.Duration());
            }
            else
            {
                this.Duration = 0;
                this.CurrentTime = 0;
            }
        }

        private void MusicControlService_OnPlayStatusChanged(object sender, bool e)
        {
            this.IsPlaying = e;
        }

        public bool DoUpdate()
        {
            this.CurrentTime = GetPlatformSpecificTime(musicControlService.CurrentTime());
            this.Duration = GetPlatformSpecificTime(musicControlService.Duration());

            return true;
        }

        /// <summary>
        /// 获取指定平台的准确时间
        /// </summary>
        /// <param name="originTime"></param>
        /// <returns></returns>
        public double GetPlatformSpecificTime(double originTime)
        {
            double resultTime;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    resultTime = originTime;
                    break;
                case Device.Android:
                    resultTime = originTime / 1000;
                    break;

                case Device.UWP:
                    resultTime = originTime;
                    break;
                default:
                    resultTime = 0;
                    break;
            }
            return resultTime;

        }

        public async Task RebuildMusicInfos(Action callback =null)
        {
            await this.musicControlService.RebuildMusicInfos(callback);
        }

    }
}
