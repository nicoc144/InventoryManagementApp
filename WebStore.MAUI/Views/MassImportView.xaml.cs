using WebStore.MAUI.ViewModels;

namespace WebStore.MAUI.Views;

public partial class MassImportView : ContentPage
{
	public MassImportView()
	{
		InitializeComponent();
	}

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as InventoryManagementViewModel).AddCSV(PassedInFile.Text); //calls add or update from the ItemViewModel model
        Shell.Current.GoToAsync("//Inventory");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Inventory");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new InventoryManagementViewModel();
    }
}