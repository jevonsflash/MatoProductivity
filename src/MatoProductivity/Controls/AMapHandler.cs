using Microsoft.Maui.Handlers;

namespace MatoProductivity.Controls;
public interface IAMap : IView
{
}
partial class AMapHandler
{
    public static IPropertyMapper<IAMap, AMapHandler> MapMapper = new PropertyMapper<IAMap, AMapHandler>(ViewHandler.ViewMapper)
    { };

    public AMapHandler() : base(MapMapper)
    { }
}

