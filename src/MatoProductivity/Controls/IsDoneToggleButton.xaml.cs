using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MatoProductivity.Core.Controls;

public partial class IsDoneToggleButton : ContentView
{
    public event EventHandler<ToggledEventArgs> Toggled;

    public IsDoneToggleButton()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IsToggledProperty =
      BindableProperty.Create("IsToggled", typeof(bool), typeof(IsDoneToggleButton), false, propertyChanged: (bindable, oldValue, newValue) =>
      {
          var obj = (IsDoneToggleButton)bindable;
          obj.SetIsToggled(obj.IsToggled);

      });

    public bool IsToggled
    {
        get { return (bool)GetValue(IsToggledProperty); }
        set { SetValue(IsToggledProperty, value); }
    }

    public static readonly BindableProperty IsReadOnlyProperty =
      BindableProperty.Create("IsReadOnly", typeof(bool), typeof(IsDoneToggleButton), false);

    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    private void SetIsToggled(bool isChecked)
    {
        string visualState = isChecked ? "Checked" : "Unchecked";
        VisualStateManager.GoToState(MainBorder, visualState);
    }

    private void TapGestureRecognizer_OnTapped(object sender, TappedEventArgs e)
    {
        if (!IsReadOnly)
        {
            this.IsToggled=!this.IsToggled;
            this.Toggled?.Invoke(this, new ToggledEventArgs(IsToggled));
        }

    }
}