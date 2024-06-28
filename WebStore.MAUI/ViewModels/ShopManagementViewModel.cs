﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        //DO NOT MODIFY
        public ShoppingCart Cart
        {
            get
            {
                return ShoppingCartService.Current?.Cart ?? new ShoppingCart();
            }
        }

        public List<ItemViewModel> Contents
        {
            get
            {
                return Cart.Contents.Select(i => new ItemViewModel(i)).ToList() ?? new List<ItemViewModel>();
            }
        }

        public decimal Total
        {
            get
            {
                return Cart.ShoppingCartTotal;
            }
        }

        //private Item itemToBuy; //set this to private to ensure that we are not hitting the setter execpt for the first time
        public ItemViewModel ItemToBuy { get; set; }


        //public Item ItemToBuy { get; set; } //similar to selectedItem in inventorymanagementview code behind

        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //for NotifyPropertyChanged, you pass in the name of a property that has changed and the property refresh
        public void RefreshItems()
        {
            NotifyPropertyChanged(nameof(Items));
            NotifyPropertyChanged(nameof(Contents));
            NotifyPropertyChanged(nameof(Total));
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
