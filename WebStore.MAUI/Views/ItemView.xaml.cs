using WebStore.Library.Models;
using WebStore.MAUI.ViewModels;

namespace WebStore.MAUI.Views;

//Square brackets means decorator, it looks for something with the name on the right and 
//tries to put it into a property that has the name on the left. In this case it recieves the item id.
//Side note, you use nameof() because if you just passed in "ItemID" it wouldnt give you an error
//if there was an issue with finding "ItemID". Changes a hard to find runtime error into a compile time error.
//(recieves the itemID from Shell.Current.GoToAsync()
[QueryProperty(nameof(ItemID), "itemID")]

public partial class ItemView : ContentPage
{
    public int ItemID { get; set; } //This property ItemID is set by the QueryProperty.
                                    //Set to 0 initially if the item is an add, and set
                                    //to the selected item id if the item is an edit.
	public ItemView()
	{
		InitializeComponent();
	}

	private void OkClicked(object sender, EventArgs e)
	{
        (BindingContext as ItemViewModel).Add(); //calls add or update from the ItemViewModel model
        Shell.Current.GoToAsync("//Inventory");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Inventory");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e) //set a new binding context every time you navigate to this view
    {
        BindingContext = new ItemViewModel(ItemID); //Calls constructor for item view model passing in the item id as a parameter
                                                    //where ItemID was retrieved from itemID by the query.
                                                    //If it's an add the item ID will be null, if its an update the item
                                                    //ID will be the id of the item you want to update.
    }

}