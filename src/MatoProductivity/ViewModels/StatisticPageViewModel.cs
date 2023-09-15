using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.MicroKernel.Registration;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Infrastructure.Helper;
using MatoProductivity.Models;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Common;
using System.Diagnostics;
using System.Windows.Input;

namespace MatoProductivity.ViewModels
{
    public class StatisticPageViewModel : ViewModelBase, ISingletonDependency
    {
        private readonly IDateService _dateService;
        private readonly IRepository<Note, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;
        private NotePage notePagePage;

        public StatisticPageViewModel(
            IDateService dateService,
            IRepository<Note, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService)
        {
            this._dateService=dateService;
            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += NotePageViewModel_PropertyChangedAsync;
            this.Create = new Command(CreateActionAsync);
            this.Edit = new Command(EditAction);
            this.Remove = new Command(RemoveAction);
            this.RemoveSelected = new Command(RemoveSelectedAction);
            this.SelectAll = new Command(SelectAllAction);
            this.Search = new Command(SearchAction);
            this.SwitchState=new Command(SwitchStateAction);
            PreviousWeekCommand = new Command<DateTime>(PreviousWeekCommandHandler);
            NextWeekCommand = new Command<DateTime>(NextWeekCommandHandler);
            DayCommand = new Command<DayModel>(DayCommandHandler);

            SelectedNotes = new ObservableCollection<object>();

            //Init();
        }
        private DayModel _selectedDay;


        private ObservableCollection<DayModel> _daysList;

        public ObservableCollection<DayModel> DaysList
        {
            get { return _daysList; }
            set
            {
                _daysList = value;
                RaisePropertyChanged();

            }
        }


        private WeekModel _week;

        public WeekModel Week
        {
            get { return _week; }
            set
            {
                _week = value;
                RaisePropertyChanged();

            }
        }



        private void DayCommandHandler(DayModel day)
        {
            SetActiveDay(day);
            //todo
            //CreateQueryForTasks(day.Date);
        }

        private void PreviousWeekCommandHandler(DateTime startDate)
        {
            Week = _dateService.GetWeek(startDate.AddDays(-1));
            DaysList = new ObservableCollection<DayModel>(_dateService.GetDayList(Week.StartDay, Week.LastDay));
            SetActiveDay();
        }

        private void NextWeekCommandHandler(DateTime lastDate)
        {
            Week = _dateService.GetWeek(lastDate.AddDays(1));
            DaysList = new ObservableCollection<DayModel>(_dateService.GetDayList(Week.StartDay, Week.LastDay));
            SetActiveDay();
        }

        private void SetActiveDay(DayModel day = null)
        {
            ResetActiveDay();
            if (day != null)
            {
                _selectedDay = day;
                day.State = DayStateEnum.Active;
            }
            else
            {
                var selectedDate = DaysList.FirstOrDefault(d => d.Date == _selectedDay.Date);
                if (selectedDate != null)
                {
                    selectedDate.State = DayStateEnum.Active;
                }
            }
        }

        private void ResetActiveDay()
        {
            var selectedDay = DaysList?.FirstOrDefault(d => d.State.Equals(DayStateEnum.Active));
            if (selectedDay != null)
            {
                selectedDay.State = selectedDay.Date < DateTime.Now.Date ? DayStateEnum.Past : DayStateEnum.Normal;
            }
        }

        private void SwitchStateAction(object obj)
        {
            this.IsEditing= !this.IsEditing;
        }

        private void SearchAction(object obj)
        {
            this.Init();
        }

        private void SelectAllAction(object obj)
        {
            foreach (var notes in Notes)
            {
                SelectedNotes.Add(notes);
            }
        }

        private void RemoveSelectedAction(object obj)
        {
            foreach (var note in SelectedNotes.ToList())
            {
                foreach (var noteGroup in this.NoteGroups)
                {
                    var delete = noteGroup.FirstOrDefault(c => c.Id == (note as Note).Id);
                    noteGroup.Remove(delete);
                }
            }

        }

        private void RemoveAction(object obj)
        {
            var note = (Note)obj;
            foreach (var noteGroup in this.NoteGroups)
            {
                var delete = noteGroup.FirstOrDefault(c => c.Id == note.Id);
                noteGroup.Remove(delete);
            }

        }

        private async void EditAction(object obj)
        {
            var note = (Note)obj;

            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = note.Id }))
            {
                await navigationService.PushAsync(objWrapper.Object);
            }
        }

        private async void CreateActionAsync(object obj)
        {

            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = 0 }))
            {
                (objWrapper.Object.BindingContext as EditNotePageViewModel).Create.Execute(null);

                await navigationService.PushAsync(objWrapper.Object);
            }
        }

        public void Init()
        {
            Week = _dateService.GetWeek(DateTime.Now);
            DaysList = new ObservableCollection<DayModel>(_dateService.GetDayList(Week.StartDay, Week.LastDay));
            _selectedDay = new DayModel() { Date = DateTime.Today };

            var notes = this.repository.GetAllList()
                .WhereIf(!string.IsNullOrEmpty(this.SearchKeywords), c => c.Title.Contains(this.SearchKeywords));
            var notegroupedlist = notes.OrderByDescending(c => c.CreationTime).GroupBy(c => CommonHelper.FormatTimeString(c.LastModificationTime==null ? c.CreationTime : c.LastModificationTime.Value)).Select(c => new NoteTimeLineGroup(c.Key, c));
            this.NoteGroups = new ObservableCollection<NoteTimeLineGroup>(notegroupedlist);

            foreach (var noteGroups in this.NoteGroups)
            {
                noteGroups.CollectionChanged += Notes_CollectionChanged;
            }
        }

        private async void Notes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    await this.repository.DeleteAsync((item as Note).Id);

                }
            }
        }

        private async void NotePageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedNote))
            {
                if (SelectedNote != default)
                {
                    using (var objWrapper = iocResolver.ResolveAsDisposable<NotePage>(new { NoteId = SelectedNote.Id }))
                    {
                        notePagePage = objWrapper.Object;
                        (notePagePage.BindingContext as NotePageViewModel).OnDone += StatisticPageViewModel_OnDone; ;

                        await navigationService.ShowPopupAsync(notePagePage);
                    }
                    SelectedNote = default;
                }
            }

            //else if (e.PropertyName == nameof(SearchKeywords))
            //{
            //    if (string.IsNullOrEmpty(SearchKeywords))
            //    {
            //        Init();
            //    }
            //}
        }

        private async void StatisticPageViewModel_OnDone(object sender, EventArgs e)
        {
            await navigationService.HidePopupAsync(notePagePage);
            this.Init();

        }

        private ObservableCollection<NoteTimeLineGroup> _noteGroups;

        public ObservableCollection<NoteTimeLineGroup> NoteGroups
        {
            get { return _noteGroups; }
            set
            {
                _noteGroups = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Notes));
            }
        }





        public IEnumerable<Note> Notes => NoteGroups.SelectMany(c => c);


        private Note _selectedNote;

        public Note SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                RaisePropertyChanged();

            }
        }


        private ObservableCollection<object> _selectedNotes;

        public ObservableCollection<object> SelectedNotes
        {
            get { return _selectedNotes; }
            set
            {
                _selectedNotes = value;
                RaisePropertyChanged();

            }
        }

        private bool _isEditing;

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectionMode));

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



        public SelectionMode SelectionMode => IsEditing ? SelectionMode.Multiple : SelectionMode.Single;


        public Command Create { get; set; }
        public Command Remove { get; set; }
        public Command Edit { get; set; }
        public Command RemoveSelected { get; set; }
        public Command SelectAll { get; set; }
        public Command Search { get; set; }
        public Command SwitchState { get; set; }

        public Command DayCommand { get; set; }
        public Command PreviousWeekCommand { get; set; }
        public Command NextWeekCommand { get; set; }
    }
}
