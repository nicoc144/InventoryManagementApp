using WebStore.MAUI.ViewModels;

namespace WebStore.MAUI.Views;

[QueryProperty(nameof(CartID), "cartID")]

public partial class CartView : ContentPage
{
    public int CartID { get; set; }
    public CartView()
	{

		InitializeComponent();
	}

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as CartViewModel).AddCart(); //update the details of the cart, (pretty much the name only)
        Shell.Current.GoToAsync("//CartManagementView");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CartManagementView");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new CartViewModel(CartID);
    }
}