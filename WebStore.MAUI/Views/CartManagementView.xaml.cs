using WebStore.Library.Models;
using WebStore.MAUI.ViewModels;

namespace WebStore.MAUI.Views;

public partial class CartManagementView : ContentPage
{
    public CartManagementView()
    {
        InitializeComponent();
        BindingContext = new CartManagementViewModel();
    }

    private void CartBack(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Shop");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as CartManagementViewModel).RefreshCarts();
    }

    private void SetAsActiveShoppingCartClicked(object sender, EventArgs e)
    {
        //set the current shopping cart as the active cart, update these changes in the shop management view
        (BindingContext as CartManagementViewModel).SetActiveCart();
    }

    private void EditShoppingCartNameClicked(object sender, EventArgs e)
    {
        (BindingContext as CartManagementViewModel).UpdateCart();
    }

    private void CreateNewCartClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CartView");
    }

    private void DeleteShoppingCartClicked(object sender, EventArgs e)
    {
        (BindingContext as CartManagementViewModel).DeleteShoppingCart();
    }
}