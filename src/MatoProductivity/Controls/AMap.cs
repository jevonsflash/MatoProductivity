using MatoProductivity.Controls;
using Microsoft.Maui.Handlers;

namespace MatoProductivity.Core.Controls;
public class AMap : View, IAMap
{
    public static readonly BindableProperty AddressProperty =
      BindableProperty.Create("Address", typeof(string), typeof(AMap), default, propertyChanged: (bindable, oldValue, newValue) =>
      {
          var obj = (AMap)bindable;
          var newAddress = newValue as string;
      });

    public string Address
    {
        get { return (string)GetValue(AddressProperty); }
        set { SetValue(AddressProperty, value); }
    }

    public static readonly BindableProperty LocationProperty =
  BindableProperty.Create("Location", typeof(MatoProductivity.Core.Location.Location), typeof(AMap), default, propertyChanged: (bindable, oldValue, newValue) =>
  {
      var obj = (AMap)bindable;
      var newLocation = newValue as MatoProductivity.Core.Location.Location;
  });

    public MatoProductivity.Core.Location.Location Location
    {
        get { return (MatoProductivity.Core.Location.Location)GetValue(LocationProperty); }
        set { SetValue(LocationProperty, value); }
    }
}

