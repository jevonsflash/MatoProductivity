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
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class LocationSegmentService : NoteSegmentService, ITransientDependency, IPopupContainerViewModelBase, IAutoSet
    {

        private readonly AmapHttpRequestClient amapHttpRequestClient;
        private readonly NavigationService navigationService;
        private readonly IIocResolver iocResolver;
        public event EventHandler<AutoSetChangedEventArgs> OnAutoSetChanged;
        public Command PickFromMap { get; set; }
        private Popup locationSelectingPage;
        public LocationSegmentService(
            AmapHttpRequestClient amapHttpRequestClient,
            NavigationService navigationService,
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            INoteSegment noteSegment, IIocResolver iocResolver) : base(repository, payloadRepository, noteSegment)
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
                var title = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();
                var location = await GeoLocationHelper.GetNativePosition();
                var amapLocation = new Core.Location.Location()
                {
                    Latitude=location.Latitude,
                    Longitude=location.Longitude
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
                    Logger.Error(ex.ToString);
                }
                if (reGeocodeLocation==null)
                {
                    return;
                }
                var address = reGeocodeLocation.Address;

                var defaultLocationSegmentPayload = this.CreateNoteSegmentPayload(nameof(Location), amapLocation.ToString());
                this.Location = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Location), defaultLocationSegmentPayload)?.GetStringValue();

                var defaultAddressSegmentPayload = this.CreateNoteSegmentPayload(nameof(Address), address);
                this.Address = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Address), defaultAddressSegmentPayload)?.GetStringValue();
            }

            else if (e.PropertyName == nameof(Address))
            {
                if (!string.IsNullOrEmpty(Address))
                {
                    NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Address), Address));

                }
            }

            else if (e.PropertyName == nameof(Location))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Location), Location));
            }
            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Title), Title));
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

            (sender as LocationSelectingPageViewModel).OnFinishedChooise -= LocationSelectingPageViewModel_OnFinishedChooise;
            await navigationService.HidePopupAsync(locationSelectingPage);
        }

        public override void CreateAction(object obj)
        {

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
