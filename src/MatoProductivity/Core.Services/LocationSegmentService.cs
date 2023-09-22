using Abp.Dependency;
using Abp.Domain.Repositories;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Core.Views;
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

        public Command PickFromMap { get; set; }
        private ContentPage locationSelectingPage;
        private INoteSegmentPayload DefaultContentSegmentPayload => this.CreateNoteSegmentPayload(nameof(Content), "");
        public LocationSegmentService(
            NavigationService navigationService,
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            INoteSegment noteSegment, IIocResolver iocResolver) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += LocationSegmentViewModel_PropertyChanged;
            PickFromMap = new Command(PickFromMapAction);
            this.navigationService=navigationService;
            this.iocResolver=iocResolver;
        }

        private void LocationSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();


                var content = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Content), DefaultContentSegmentPayload);
                Content = content.GetStringValue();

                var defaultPlaceHolderSegmentPayload = this.CreateNoteSegmentPayload(nameof(PlaceHolder), "请输入" + Title);

                var placeHolder = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(PlaceHolder), defaultPlaceHolderSegmentPayload);
                PlaceHolder = placeHolder.GetStringValue();
            }

            else if (e.PropertyName == nameof(Content))
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Content), Content));

                }
            }

            else if (e.PropertyName == nameof(PlaceHolder))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(PlaceHolder), PlaceHolder));
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

        private string _placeHolder;
        private readonly NavigationService navigationService;
        private readonly IIocResolver iocResolver;

        public string PlaceHolder
        {
            get { return _placeHolder; }
            set
            {
                _placeHolder = value;
                RaisePropertyChanged();
            }
        }




    }
}
