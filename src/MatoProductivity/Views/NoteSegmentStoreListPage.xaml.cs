using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class NoteSegmentStoreListPage : PopupBase, ITransientDependency
{
    private NoteSegmentStoreListPageViewModel NoteSegmentStoreListPageViewModel => this.BindingContext as NoteSegmentStoreListPageViewModel;

    public NoteSegmentStoreListPage(NoteSegmentStoreListPageViewModel noteSegmentStoreListPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = noteSegmentStoreListPageViewModel;
    }
}