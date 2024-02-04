using Abp.Dependency;
using Abp.Domain.Repositories;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Amap;
using MatoProductivity.Core.Location;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Core.Views;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MatoProductivity.Core.Services
{
    public class LocationSegmentService : NoteSegmentService, ITransientDependency, IPopupContainerViewModelBase, IAutoSet
    {

        private Location.Location amapLocation;
        private readonly AmapHttpRequestClient amapHttpRequestClient;
        private readonly NavigationService navigationService;
        private readonly IIocResolver iocResolver;
        public event EventHandler<AutoSetChangedEventArgs> OnAutoSetChanged;
        public Command PickFromMap { get; set; }
        private Popup locationSelectingPage;
        public LocationSegmentService(
            AmapHttpRequestClient amapHttpRequestClient,
            NavigationService navigationService,
            INoteSegment noteSegment, IIocResolver iocResolver) : base(noteSegment)
        {
            PropertyChanged += LocationSegmentViewModel_PropertyChanged;
            PickFromMap = new Command(PickFromMapAction);
            this.amapHttpRequestClient=amapHttpRequestClient;
            this.navigationService=navigationService;
            this.iocResolver=iocResolver;
        }

        private async void LocationSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Title), defaultTitle);
                Title = title.GetStringValue();


                var location = NoteSegment?.GetNoteSegmentPayload(nameof(Location));
                if (location!=null)
                {
                    this.Location = location.GetStringValue();
                }
                else
                {
                    INoteSegmentPayload defaultLocationSegmentPayload;
                    amapLocation = await this.GetLocation();
                    if (amapLocation!=null)
                    {
                        defaultLocationSegmentPayload = this.CreateNoteSegmentPayload(nameof(Location), amapLocation.ToFriendlyString());
                        this.Location = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Location), defaultLocationSegmentPayload)?.GetStringValue();
                    }

                }


                var address = NoteSegment?.GetNoteSegmentPayload(nameof(Address));
                if (address!=null)
                {
                    this.Address = address.GetStringValue();
                }
                else
                {
                    INoteSegmentPayload defaultAddressSegmentPayload;

                    // amapLocation ??= await this.GetLocation();
                    if (amapLocation!=null)
                    {
                        var newAddress = await this.GetAdress(amapLocation);
                        if (newAddress!=null)
                        {
                            defaultAddressSegmentPayload = this.CreateNoteSegmentPayload(nameof(Address), newAddress);
                            this.Address = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Address), defaultAddressSegmentPayload)?.GetStringValue();
                        }
                    }
                }

            }

            else if (e.PropertyName == nameof(Address))
            {
                if (!string.IsNullOrEmpty(Address))
                {
                    NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Address), Address));

                }

            }

            else if (e.PropertyName == nameof(Location))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Location), Location));
            }
            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Title), Title));
            }
        }


        private async void PickFromMapAction(object obj)
        {
            PopupLoading = true;
            PickFromMap.ChangeCanExecute();

            using (var objWrapper = iocResolver.ResolveAsDisposable<LocationSelectingPage>())
            {
                locationSelectingPage = objWrapper.Object;
                (locationSelectingPage.BindingContext as LocationSelectingPageViewModel).OnFinishedChooise += LocationSelectingPageViewModel_OnFinishedChooise;
            }

            await navigationService.ShowPopupAsync(locationSelectingPage).ContinueWith(async (e) =>
            {
                (locationSelectingPage.BindingContext as LocationSelectingPageViewModel).OnFinishedChooise -= LocationSelectingPageViewModel_OnFinishedChooise;
                locationSelectingPage = null;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PopupLoading = false;
                    PickFromMap.ChangeCanExecute();
                });

            }); ;

        }


        private async void LocationSelectingPageViewModel_OnFinishedChooise(object sender, FinishedChooiseEvenArgs args)
        {
            this.Address=args.Address;
            this.Location=args.Location?.ToFriendlyString();

            (sender as LocationSelectingPageViewModel).OnFinishedChooise -= LocationSelectingPageViewModel_OnFinishedChooise;
            await navigationService.HidePopupAsync(locationSelectingPage);
        }
        public override async void CreateAction(object obj)
        {

        }
        private async Task<Core.Location.Location> GetLocation()
        {
            Location.Location amapLocation = null;
            if (await CheckPermissionIsGrantedAsync<LocationWhenInUse>("请在设置中开启位置的访问权限"))
            {
                var location = await GeoLocationHelper.GetNativePosition();
                if (location==null)
                {
                    return null;
                }
                amapLocation = new Core.Location.Location()
                {
                    Latitude=location.Latitude,
                    Longitude=location.Longitude
                };

            }
            return amapLocation;

        }
        private async Task<string> GetAdress(Location.Location amapLocation)
        {
            string address = null;

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
                Logger.Error(ex.ToString);
            }
            if (reGeocodeLocation==null)
            {
                return null;
            }
            address = reGeocodeLocation.Address;
            return address;
        }

        public async Task CloseAllPopup()
        {
            await navigationService.HidePopupAsync(locationSelectingPage);
        }


        private bool _popupLoading;

        public bool PopupLoading
        {
            get { return _popupLoading; }
            set
            {
                _popupLoading = value;
                RaisePropertyChanged();

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




        private string _location;

        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoSet { get; set; } = true;



    }
}
