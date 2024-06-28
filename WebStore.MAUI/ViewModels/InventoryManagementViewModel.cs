using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using WebStore.Library.Models;
using WebStore.Library.Services;

namespace WebStore.MAUI.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged //overall management for the inventory's data
    {
        //implementation below redraws the grid if a change happens in the list
        public event PropertyChangedEventHandler? PropertyChanged;   

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public List<ItemViewModel> Items
        {
            get
            {
                //getting the items list, selects each item from the item list and creates a new list
                //need to specify ToList because without it would assume that it's an IEnumerable
                return ItemServiceProxy.Current?.Items?.Select(i => new ItemViewModel(i)).ToList() ?? new List<ItemViewModel>();
            }
        }
        public ItemViewModel SelectedItem {  get; set; }
        public InventoryManagementViewModel()
        {

        }

        public void RefreshItems()
        {
            NotifyPropertyChanged(nameof(Items));
        }

        public void UpdateItem()
        {
            if(SelectedItem?.Item == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//Item?itemID={SelectedItem.Item.ID}"); //asynchronously go to the //Item view and
                                                                              //set itemID to ID at SelectedItem.Item.ID
                                                                              //(Sends the item ID to QueryProperty)
            ItemServiceProxy.Current.AddOrUpdate(SelectedItem.Item); //does add or update passing in the item
        }

        public void DeleteItem()
        {
            if(SelectedItem?.Item == null)
            {
                return;
            }
            ItemServiceProxy.Current.Delete(SelectedItem.ID); //deletes passing in the id of the item
            RefreshItems(); //need to add refresh here or page doesnt update
        }
    }
}
