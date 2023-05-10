namespace MatoProductivity.Core.Services
{
    public interface IDraggableItem
    {
        bool IsBeingDraggedOver { get; set; }
        bool IsBeingDragged { get; set; }
    }
}