namespace MatoProductivity.Core.Models.Entities
{
    public interface INoteSegment
    {
        string Color { get; set; }
        string Desc { get; set; }
        string Icon { get; set; }
        bool IsHidden { get; set; }
        bool IsRemovable { get; set; }
        int Rank { get; set; }
        string Status { get; set; }
        string Title { get; set; }
        string Type { get; set; }

    }
}