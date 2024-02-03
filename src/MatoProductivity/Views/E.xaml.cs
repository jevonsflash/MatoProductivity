using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class E : PopupBase, ITransientDependency
{


    private bool isOpen = false;
    private Animation[] animations = null;
    public E()
    {
        InitializeComponent();
        this.CanBeDismissedByTappingOutsideOfPopup = false;
        Opened  +=E_Opened;
        animations=new Animation[1];
    }

    private void E_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        Init();
    }


    public async void Init()
    {
        var result = await MainImage.ScaleTo(1.5, 1000).ContinueWith(c => this.CanBeDismissedByTappingOutsideOfPopup=true);
    }



}
