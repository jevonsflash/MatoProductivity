using Abp.Dependency;
using Abp.Domain.Repositories;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Amap;
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
    public class LocationSegmentService : NoteSegmentService, ITransientDependency
    {

        private readonly AmapHttpRequestClient amapHttpRequestClient;
        private readonly NavigationService navigationService;
        private readonly IIocResolver iocResolver;
        public Command PickFromMap { get; set; }
        private ContentPage locationSelectingPage;
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
                    Locations=new Location.Location[]
                    {
                        amapLocation
                   }
                };
                var reGeocodeLocation = await amapHttpRequestClient.InverseAsync(amapInverseHttpRequestParamter);
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
            if (locationSelectingPage!=null)
            {
                (locationSelectingPage.BindingContext as LocationSelectingPageViewModel).OnFinishedChooise -= LocationSelectingPageViewModel_OnFinishedChooise;
                locationSelectingPage=null;
            }

            using (var objWrapper = iocResolver.ResolveAsDisposable<LocationSelectingPage>())
            {
                locationSelectingPage = objWrapper.Object;
                (locationSelectingPage.BindingContext as LocationSelectingPageViewModel).OnFinishedChooise += LocationSelectingPageViewModel_OnFinishedChooise;
                await navigationService.PushAsync(locationSelectingPage);
            }
        }

        private async void LocationSelectingPageViewModel_OnFinishedChooise(object sender, NoteSegmentStore noteSegmentStore)
        {


            (sender as LocationSelectingPageViewModel).OnFinishedChooise -= LocationSelectingPageViewModel_OnFinishedChooise;
            await navigationService.PopAsync();
            locationSelectingPage=null;
        }

        public override void CreateAction(object obj)
        {

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




    }
}
