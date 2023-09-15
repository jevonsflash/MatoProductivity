namespace MatoProductivity.Core.ViewModels
{
    public interface ISearchViewModel
    {
        Command Search { get; set; }
        string SearchKeywords { get; set; }
    }
}