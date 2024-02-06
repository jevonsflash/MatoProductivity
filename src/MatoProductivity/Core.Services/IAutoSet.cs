namespace MatoProductivity.Core.Services
{
    public interface IAutoSet
    {
        bool IsAutoSet { get;  }
        event EventHandler<AutoSetChangedEventArgs> OnAutoSetChanged;

    }
}