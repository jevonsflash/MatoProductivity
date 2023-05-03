using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;

namespace MatoProductivity.ViewModels
{
    public class NoteTemplatePageViewModel : ViewModelBase,ITransientDependency
    {
        private readonly IRepository<NoteSegment, long> repository;

        public NoteTemplatePageViewModel(IRepository<NoteSegment, long> repository)
        {
            this.repository = repository;
        }

        



    }
}
