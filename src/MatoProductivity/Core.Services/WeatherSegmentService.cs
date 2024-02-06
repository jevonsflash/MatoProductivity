using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Core.Weather;
using MatoProductivity.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QWeatherAPI.Result.RealTimeWeather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MatoProductivity.Core.Services
{
    public class WeatherSegmentService : NoteSegmentService, ITransientDependency, IAutoSet
    {
        public event EventHandler<AutoSetChangedEventArgs> OnAutoSetChanged;
        public WeatherSegmentService(
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += WeatherSegmentViewModel_PropertyChanged;
        }

        private async void WeatherSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Title), defaultTitle);
                Title = title.GetStringValue();

                var content = NoteSegment?.GetNoteSegmentPayload(nameof(Content));
                if (content!=null)
                {
                    Content = content.GetStringValue();
                }
                else
                {
                    INoteSegmentPayload defaultContentSegmentPayload;
                    if (await CheckPermissionIsGrantedAsync<LocationWhenInUse>("请在设置中开启位置的访问权限"))
                    {
                        var location = await GeoLocationHelper.GetNativePosition();
                        if (location!=null)
                        {
                            var locationString = $"{location.Longitude},{location.Latitude}";

                            var locationInfo = (await QWeatherAPI.GeoAPI.GetGeoAsync(locationString, QWeatherConsts.Key)).Locations[0];
                            var realTimeWeatherInfo = await QWeatherAPI.RealTimeWeatherAPI.GetRealTimeWeatherAsync(locationInfo.Lon, locationInfo.Lat, QWeatherConsts.Key);

                            this.NowWeather = realTimeWeatherInfo.Now;

                            defaultContentSegmentPayload = this.CreateNoteSegmentPayload(nameof(Content), StringifyNowWeather);

                            Content = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Content), defaultContentSegmentPayload)?.GetStringValue();

                            return;
                        }
                    }



                    //defaultContentSegmentPayload = this.CreateNoteSegmentPayload(nameof(Content), "");

                    //Content = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Content), defaultContentSegmentPayload)?.GetStringValue();

                }

            }

            else if (e.PropertyName == nameof(Content))
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Content), Content));

                }
            }

            else if (e.PropertyName == nameof(NowWeather))
            {
                this.RaisePropertyChanged(nameof(StringifyNowWeather));
            }
            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Title), Title));
            }
        }

        public async override void CreateAction(object obj)
        {


        }

        private Now _nowWeather;

        public Now NowWeather
        {
            get { return _nowWeather; }
            set
            {
                _nowWeather = value;
                RaisePropertyChanged();
            }
        }

        public string StringifyNowWeather => $"温度 {NowWeather?.Temp}°C,天气 {NowWeather?.Text}";


        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged();
            }
        }



        public bool IsAutoSet => true;
    }
}
