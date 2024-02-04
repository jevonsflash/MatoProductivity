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

    public class NoteTemplateSynchronizer :
       IEventHandler<EntityCreatedEventData<NoteTemplate>>,
       IEventHandler<EntityDeletedEventData<NoteTemplate>>, ISingletonDependency
    {
        private readonly NoteTemplateListPageViewModel noteTemplateListPageViewModel;

        public NoteTemplateSynchronizer(NoteTemplateListPageViewModel noteTemplateListPageViewModel)
        {
            this.noteTemplateListPageViewModel=noteTemplateListPageViewModel;
        }
        public void HandleEvent(EntityCreatedEventData<NoteTemplate> eventData)
        {
            var entity = eventData.Entity;
            if (entity!=null)
            {
                noteTemplateListPageViewModel.NoteTemplates?.Add(new NoteTemplateWrapper(entity) { Container = this.noteTemplateListPageViewModel });
            }
        }

        public void HandleEvent(EntityDeletedEventData<NoteTemplate> eventData)
        {
            var entity = eventData.Entity;
            if (entity!=null)
            {
                var currentOne = noteTemplateListPageViewModel.NoteTemplates?.FirstOrDefault(c => c.NoteTemplate.Id==entity.Id);
                if (currentOne!=null)
                {
                    noteTemplateListPageViewModel.NoteTemplates?.Remove(currentOne);
                }
            }
        }
    }
}
