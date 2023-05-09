namespace MatoProductivity.ViewModels
{
    public interface ISearchViewModel
    {
        Command Search { get; set; }
        string SearchKeywords { get; set; }
    }
}