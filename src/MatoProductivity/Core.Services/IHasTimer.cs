namespace MatoProductivity.Core.Services
{
    public interface IHasTimer
    {
        DateTime ExactTime { get; }
        bool IsShowFromNow { get; set; }
        DateTime Time { get; set; }
        TimeSpan TimeFromNow { get; }
        TimeSpan TimeOffset { get; set; }
    }
}