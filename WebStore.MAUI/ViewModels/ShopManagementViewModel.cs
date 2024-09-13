using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.Models;
using WebStore.Library.Services;

namespace WebStore.MAUI.ViewModels
{
    public class ShopManagementViewModel : INotifyPropertyChanged //overall management for the shop's data
    {
        public ShopManagementViewModel()
        {
            ItemToBuy = new ItemViewModel();
            SetCartByID();
            RefreshItems();
        }

        //This is the inventory, singleton list for shop view DO NOT MODIFY!!!
        public List<ItemViewModel> Items //list of ItemViewModels
        {
            get
            {
                //Getting the items list, selects each item from the item list and creates a new list.
                //You are taking each item from the items list and converting it to an ItemViewModel,
                //which is better for data binding

                //need to specify ToList because without it would assume that it's an IEnumerable

                //You can use this in shopmanagement as well because it's a singleton, you can access the inventory
                //from anywhere, and its only loaded into memory when needed
                return ItemServiceProxy.Current?.Items.Select(i => new ItemViewModel(i)).ToList() ?? new List<ItemViewModel>();

            }
        }

        public List<CartViewModel> Carts //list of carts
        {
            get
            {
                return ShoppingCartServiceProxy.Current?.Carts.Select(c => new CartViewModel(c)).ToList() ?? new List<CartViewModel>();
            }
        }

        public CartViewModel CurrentCart { get; set; } //this is the current cart being added to in the shop view

        public void SetCartByID()
        {
            if (Carts.Count <= 1) //if only the default cart is in the Carts list
            {
                CurrentCart = Carts.FirstOrDefault(c => c.ShoppingCartID == 1); //set the current cart to the first shopping cart in the list
            }
            else
            {
                CurrentCart = Carts.FirstOrDefault(c => c.ShoppingCartID == ShoppingCartServiceProxy.SelectedShoppingCartID);
            }
        }

        //private Item itemToBuy; //set this to private to ensure that we are not hitting the setter execpt for the first time
        public ItemViewModel ItemToBuy { get; set; } //"ItemVeiwModel" being set to "Item" was causing problems in the professor's example

        //public Item ItemToBuy { get; set; } //similar to selectedItem in inventorymanagementview code behind

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //for NotifyPropertyChanged, you pass in the name of a property that has changed and the property refresh
        public async void RefreshItems()
        {
            await ItemServiceProxy.Current.Get();
            await ShoppingCartServiceProxy.Current.Get();
            await ShoppingCartServiceProxy.Current.Get();
            NotifyPropertyChanged(nameof(Items));
            NotifyPropertyChanged(nameof(Carts));
            NotifyPropertyChanged(nameof(CurrentCart));
        }

        public void UpdateItemView() //needed in order to set the values for the add to cart screen
        {
            if (ItemToBuy?.Item == null)
            {
                return;
            }

            Shell.Current.GoToAsync($"//AddToCart?cartItemID={ItemToBuy.Item.ID}"); //asynchronously go to the //Item view and
                                                                                    //set cartItemID to ID at SelectedItem.Item.ID
                                                                                    //(Sends the item ID to QueryProperty)
        }

    }
}
