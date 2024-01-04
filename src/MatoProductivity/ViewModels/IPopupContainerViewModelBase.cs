using CommunityToolkit.Maui.Views;

namespace MatoProductivity.ViewModels
{
    public interface IPopupContainerViewModelBase
    {
        bool PopupLoading { get; set; }

        Task CloseAllPopup();
    }
}