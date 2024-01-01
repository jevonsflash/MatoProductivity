﻿namespace MatoProductivity.Core.Services
{
    public interface IAutoSet
    {
        bool IsAutoSet { get; set; }
        event EventHandler<AutoSetChangedEventArgs> OnAutoSetChanged;

    }
}