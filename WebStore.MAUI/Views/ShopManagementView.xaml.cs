using WebStore.MAUI.ViewModels;
namespace WebStore.MAUI.Views;


public partial class ShopManagementView : ContentPage
{
	public ShopManagementView()
	{
		InitializeComponent();
		BindingContext = new ShopManagementViewModel();
	}

    private void ShopBack(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopManagementViewModel).SetCartByID();
        (BindingContext as ShopManagementViewModel).RefreshItems();
    }

    private void AddToCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopManagementViewModel).UpdateItemView(); //updates the view for SpecifyQuantView with the correct
                                                                      //data for the item
    }

    private void MyShoppingCartsClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CartManagementView");
    }
}