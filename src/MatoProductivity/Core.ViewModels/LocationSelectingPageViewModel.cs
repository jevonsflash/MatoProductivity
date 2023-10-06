using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MatoProductivity.Core.Amap;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class LocationSelectingPageViewModel : ViewModelBase, ISingletonDependency, ISearchViewModel
    {


        private readonly AmapHttpRequestClient amapHttpRequestClient;
        public event EventHandler<NoteSegmentStore> OnFinishedChooise;

        public LocationSelectingPageViewModel(AmapHttpRequestClient amapHttpRequestClient)
        {
            this.Search = new Command(SearchAction);
            this.amapHttpRequestClient=amapHttpRequestClient;
            this.PropertyChanged+=LocationSelectingPageViewModel_PropertyChanged;
            Init();
        }


        private void SearchAction(object obj)
        {
            this.Init();
        }


        private async void LocationSelectingPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(SearchKeywords))
            {
                if (string.IsNullOrEmpty(SearchKeywords))
                {
                    Init();
                }
            }
            OnFinishedChooise?.Invoke(this, null);

        }

        private async void Init()
        {
            var location = await GeoLocationHelper.GetNativePosition();
            var amapLocation = new Core.Location.Location()
            {
                Latitude=location.Latitude,
                Longitude=location.Longitude
            };
            var amapInverseHttpRequestParamter = new AmapInverseHttpRequestParamter()
            {
                Locations=new MatoProductivity.Core.Location.Location[]
                {
                        amapLocation
               }
            };
            var reGeocodeLocation = await amapHttpRequestClient.InverseAsync(amapInverseHttpRequestParamter);


        }

        private Core.Location.Location _currentLocation;

        public Core.Location.Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<NoteSegmentStoreGroup> _noteSegmentStoreGroups;

        public ObservableCollection<NoteSegmentStoreGroup> NoteSegmentStoreGroups
        {
            get { return _noteSegmentStoreGroups; }
            set
            {
                _noteSegmentStoreGroups = value;
                RaisePropertyChanged();
            }
        }

        private NoteSegmentStore _selectedNoteSegmentStore;

        public NoteSegmentStore SelectedNoteSegmentStore
        {
            get { return _selectedNoteSegmentStore; }
            set
            {
                _selectedNoteSegmentStore = value;
                RaisePropertyChanged();

            }
        }


        private string _searchKeywords;

        public string SearchKeywords
        {
            get { return _searchKeywords; }
            set
            {
                _searchKeywords = value;
                RaisePropertyChanged();

            }
        }

        public Command Search { get; set; }

    }

}
