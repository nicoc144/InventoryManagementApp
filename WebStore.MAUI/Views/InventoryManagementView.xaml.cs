using WebStore.MAUI.ViewModels;
namespace WebStore.MAUI.Views;

public partial class InventoryManagementView : ContentPage
{
	public InventoryManagementView()
	{
        InitializeComponent();
        //BindingContext essensially tells xaml where to find the backend code you want
        BindingContext = new InventoryManagementViewModel();
	}

    private void InvBack(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as InventoryManagementViewModel).RefreshItems();
    }

    private void EditClicked(object sender, EventArgs e)
    {
        //calling the UpdateItem function from the InventoryManagementViewModel, you basically
        //go to the InventoryManagementViewModel which already has access to the selected item
        //and you don't have to pass anything into the function because edit clicked is now
        //bound to the ViewModel behind the scenes
        (BindingContext as InventoryManagementViewModel).UpdateItem();

    }

    private void AddClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Item");
    }

    private void DeleteClicked(object sender, EventArgs e)
    {
        (BindingContext as InventoryManagementViewModel).DeleteItem();
    }

    private void ConfigTaxClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//TaxConfig");
    }

    private void MassImportClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MassImportView");
    }
}