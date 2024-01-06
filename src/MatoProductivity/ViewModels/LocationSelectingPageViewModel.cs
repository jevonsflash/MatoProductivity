using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MatoProductivity.Core.Amap;
using MatoProductivity.Core.Location;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Infrastructure.ThrottleDebounce;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.Maui.Devices.Sensors;
using Nito.AsyncEx;
using System;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class LocationSelectingPageViewModel : PopupViewModelBase, ISingletonDependency, ISearchViewModel
    {


        private readonly AmapHttpRequestClient amapHttpRequestClient;
        public event EventHandler<FinishedChooiseEvenArgs> OnFinishedChooise;
        private static AsyncLock asyncLock = new AsyncLock();
        public static RateLimitedAction throttledAction = Debouncer.Debounce(null, TimeSpan.FromMilliseconds(500), leading: false, trailing: true);
        public LocationSelectingPageViewModel(AmapHttpRequestClient amapHttpRequestClient)
        {
            this.Search = new Command(SearchAction);
            this.Done = new Command(DoneAction);
            this.Remove = new Command(RemoveAction);
            this.amapHttpRequestClient=amapHttpRequestClient;
            this.PropertyChanged+=LocationSelectingPageViewModel_PropertyChanged;
            Init();
        }

        private void RemoveAction(object obj)
        {
        }

        private void DoneAction(object obj)
        {
            OnFinishedChooise?.Invoke(this, new FinishedChooiseEvenArgs(this.Address, this.CurrentLocation));

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

            else if (e.PropertyName == nameof(this.CurrentLocation))
            {
                if (CurrentLocation!=null)
                {

                    // 使用防抖
                    using (await asyncLock.LockAsync())
                    {

                        var amapLocation = new Core.Location.Location()
                        {
                            Latitude=this.CurrentLocation.Latitude,
                            Longitude=this.CurrentLocation.Longitude
                        };
                        var amapInverseHttpRequestParamter = new AmapInverseHttpRequestParamter()
                        {
                            Locations= [amapLocation]
                        };
                        ReGeocodeLocation reGeocodeLocation = null;
                        try
                        {
                            reGeocodeLocation = await amapHttpRequestClient.InverseAsync(amapInverseHttpRequestParamter);
                        }
                        catch (Exception ex)
                        {

                            Logger.Warn(ex.ToString());
                        }

                        throttledAction.Update(() =>
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                this.CurrentLocation=amapLocation;
                                Address = reGeocodeLocation?.Address;
                                Pois=new ObservableCollection<Core.Location.Poi>(reGeocodeLocation.Pois);
                            });
                        });
                        throttledAction.Invoke();
                    }
                }
            }


        }

        private async void Init()
        {
            var location = await GeoLocationHelper.GetNativePosition();

            var amapLocation = new Core.Location.Location()
            {
                Latitude=location.Latitude,
                Longitude=location.Longitude
            };
            this.CurrentLocation=amapLocation;

        }

        private Core.Location.Location _currentLocation;

        public Core.Location.Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {

                if (_currentLocation != value)
                {
                    if (value!=null &&_currentLocation!=null&&Core.Location.Location.CalcDistance(value, _currentLocation)<100)
                    {
                        return;
                    }

                    _currentLocation = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _address;

        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<Core.Location.Poi> _pois;

        public ObservableCollection<Core.Location.Poi> Pois
        {
            get { return _pois; }
            set
            {
                _pois = value;
                RaisePropertyChanged();
            }
        }

        private Core.Location.Poi _selectedPoi;

        public Core.Location.Poi SelectedPoi
        {
            get { return _selectedPoi; }
            set
            {
                _selectedPoi = value;
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
        public Command Done { get; set; }
        public Command Remove { get; set; }

    }

}
