using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Services
{

    public class NoteSynchronizer :
       IEventHandler<EntityCreatedEventData<Note>>,
       IEventHandler<EntityUpdatedEventData<Note>>,
       IEventHandler<EntityDeletedEventData<Note>>, ISingletonDependency
    {
        private readonly NoteListPageViewModel noteListPageViewModel;

        public NoteSynchronizer(NoteListPageViewModel noteListPageViewModel)
        {
            this.noteListPageViewModel=noteListPageViewModel;
        }
        public void HandleEvent(EntityCreatedEventData<Note> eventData)
        {
            var entity = eventData.Entity;
            if (entity!=null)
            {
                var currentGroup = noteListPageViewModel.NoteGroups?.FirstOrDefault(c => c.TimeCategory=="刚刚");
                if (currentGroup==null)
                {
                    noteListPageViewModel.NoteGroups?.Insert(0, new NoteTimeLineGroup("刚刚", [entity]));
                }
                else
                {
                    currentGroup.Insert(0, entity);

                }

            }
        }

        public void HandleEvent(EntityDeletedEventData<Note> eventData)
        {
            var entity = eventData.Entity;
            if (entity!=null)
            {
                var currentOne = noteListPageViewModel.Notes?.FirstOrDefault(c => c.Id==entity.Id);
                if (currentOne!=null && noteListPageViewModel.NoteGroups!=null)
                {
                    foreach (var noteGroup in noteListPageViewModel.NoteGroups)
                    {
                        noteGroup.Remove(currentOne);
                    }
                }
            }
        }

        public void HandleEvent(EntityUpdatedEventData<Note> eventData)
        {
            var entity = eventData.Entity;
            if (entity!=null)
            {
                var currentOne = noteListPageViewModel.Notes?.FirstOrDefault(c => c.Id==entity.Id);
                if (currentOne!=null)
                {
                    currentOne=entity;
                }
            }
        }

    }
}
