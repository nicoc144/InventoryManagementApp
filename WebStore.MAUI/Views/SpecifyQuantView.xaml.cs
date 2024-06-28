using WebStore.Library.Models;
using WebStore.MAUI.ViewModels;
namespace WebStore.MAUI.Views;

[QueryProperty(nameof(CartItemID), "cartItemID")]

public partial class SpecifyQuantView : ContentPage
{
    public int CartItemID { get; set; }
    public SpecifyQuantView()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ItemViewModel(CartItemID, "cloneThisID");
    }
    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ItemViewModel).AddOrUpdateCart();
        Shell.Current.GoToAsync("//Shop");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Shop");
    }
}