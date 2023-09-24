﻿using Microsoft.Maui.Handlers;

namespace MatoProductivity.Controls;
public interface IAMap : IView
{
    string Address { get; set; }
    MatoProductivity.Core.Location.Location Location { get; set; }    
}
partial class AMapHandler
{
    public static IPropertyMapper<IAMap, AMapHandler> PropertyMapper = new PropertyMapper<IAMap, AMapHandler>(ViewHandler.ViewMapper)
    {
        [nameof(IAMap.Address)] = MapAddress,
        [nameof(IAMap.Location)] = MapLocation
    };

    public static CommandMapper<IAMap, AMapHandler> CommandMapper = new(ViewCommandMapper)
    {
    };

    public AMapHandler() : base(PropertyMapper, CommandMapper)
    { }
}

