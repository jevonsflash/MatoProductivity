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
                var title = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();

                var location = await GeoLocationHelper.GetNativePosition();

                var locationString = $"{location.Longitude},{location.Latitude}";
                var locationInfo = (await QWeatherAPI.GeoAPI.GetGeoAsync(locationString, QWeatherConsts.Key)).Locations[0];
                var realTimeWeatherInfo = await QWeatherAPI.RealTimeWeatherAPI.GetRealTimeWeatherAsync(locationInfo.Lon, locationInfo.Lat, QWeatherConsts.Key);

                this.NowWeather = realTimeWeatherInfo.Now;

                var defaultContentSegmentPayload = this.CreateNoteSegmentPayload(nameof(Content), StringifyNowWeather);

                var content = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Content), defaultContentSegmentPayload);
                Content = content.GetStringValue();

            }

            else if (e.PropertyName == nameof(Content))
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Content), Content));

                }
            }

            else if (e.PropertyName == nameof(NowWeather))
            {
                this.RaisePropertyChanged(nameof(StringifyNowWeather));
            }
            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Title), Title));
            }
        }

        public async override void CreateAction(object obj)
        {

            var location = await GeoLocationHelper.GetNativePosition();

            var locationString = $"{location.Longitude},{location.Latitude}";
            var locationInfo = (await QWeatherAPI.GeoAPI.GetGeoAsync(locationString, QWeatherConsts.Key)).Locations[0];
            var realTimeWeatherInfo = await QWeatherAPI.RealTimeWeatherAPI.GetRealTimeWeatherAsync(locationInfo.Lon, locationInfo.Lat, QWeatherConsts.Key);

            this.NowWeather = realTimeWeatherInfo.Now;

            var defaultContentSegmentPayload = this.CreateNoteSegmentPayload(nameof(Content), StringifyNowWeather);

            NoteSegment?.SetNoteSegmentPayloads(defaultContentSegmentPayload);
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

        public string StringifyNowWeather => $"温度 {NowWeather.Temp}°C,天气 {NowWeather.Text}";


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

        private string _title;


        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoSet { get; set; } = true;
    }
}
