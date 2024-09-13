using WebStore.MAUI.ViewModels;

namespace WebStore.MAUI.Views;

[QueryProperty(nameof(CartRemoveItemID), "cartRemoveItemID")]

public partial class SpecifyQuantToRemoveView : ContentPage
{
	public int CartRemoveItemID { get; set; }
    public SpecifyQuantToRemoveView()
	{
		InitializeComponent();
	}

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ItemViewModel).RemoveItemFromCart();
        Shell.Current.GoToAsync("//Shop");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Shop");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ItemViewModel(CartRemoveItemID, 1);
    }
}