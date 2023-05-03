﻿using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class EditNotePageViewModel : ViewModelBase, ITransientDependency
    {
        private readonly IRepository<NoteSegment, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public EditNotePageViewModel(IRepository<NoteSegment, long> repository, IUnitOfWorkManager unitOfWorkManager)
        {
            Submit = new Command(SubmitAction);
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.PropertyChanged += EditNotePageViewModel_PropertyChanged;
        }

        private async void EditNotePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteId))
            {
                if (NoteId != default)
                {

                    await unitOfWorkManager.WithUnitOfWorkAsync( async() =>
                    {
                        var noteSegments = await this.repository
                  .GetAllIncluding(c => c.NoteSegmentPayloads)
                  .Where(c => c.Id == this.NoteId).ToListAsync();

                        this.NoteSegments = new ObservableCollection<NoteSegment>(noteSegments);
                    });
                }
            }


        }

        private long noteId;

        public long NoteId
        {
            get { return noteId; }
            set
            {
                noteId = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<NoteSegment> _noteSegments;

        public ObservableCollection<NoteSegment> NoteSegments
        {
            get { return _noteSegments; }
            set
            {
                _noteSegments = value;
                RaisePropertyChanged();
            }
        }
        private async void SubmitAction(object obj)
        {
            foreach (var noteSegment in NoteSegments)
            {
                await this.repository.InsertOrUpdateAsync(noteSegment);

            }

        }
        public Command Submit { get; set; }

    }
}
