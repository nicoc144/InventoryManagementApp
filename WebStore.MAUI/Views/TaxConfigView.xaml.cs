using WebStore.MAUI.ViewModels;

namespace WebStore.MAUI.Views;

public partial class TaxConfigView : ContentPage
{
	public TaxConfigView()
	{
		InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ShopManagementViewModel();
    }
    private void OkClicked(object sender, EventArgs e)
    {
        //Could probably also make it so a "cancel" can be added, but the only way I'd know how to do that is
        //if I had a list of Carts like how I have a list of items. Since I would then be able
        //to set the binding context to the ID of the shopping cart I would then be able to update the
        //Tax of each individual cart with a function and if I clicked cancel it wouldn't update.
        //Right now I just have the tax value being configured by a smart pointer to the tax property.
        Shell.Current.GoToAsync("//Inventory");
    }
}