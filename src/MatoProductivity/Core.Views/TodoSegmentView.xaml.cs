using Abp.Dependency;
using MatoProductivity.Core.Services;
using Microsoft.Maui.Layouts;

namespace MatoProductivity.Core.Views;

public partial class TodoSegmentView : ContentView, ITransientDependency
{

    public TodoSegmentView()
    {
        InitializeComponent();
    }

    private void IsDoneToggleButton_Toggled(object sender, ToggledEventArgs e)
    {
        (this.BindingContext as TodoSegmentService).SwitchDone.Execute(e.Value);
    }
}