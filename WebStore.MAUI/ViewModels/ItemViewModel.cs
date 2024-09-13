
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebStore.Library.DTO;
using WebStore.Library.Models;
using WebStore.Library.Services;
using Windows.Devices.Bluetooth.Advertisement;

namespace WebStore.MAUI.ViewModels
{
    public class ItemViewModel //This simply handles stuff that doesn't belong in the models file for item.cs which is only for data and buisness logic.
                               //Presentation logic for a single item.
    {
        public ICommand? EditCommand { get; set; } //in xaml you can only bind to properties, so this property is used
        public ICommand? DeleteCommand { get; set; }
        public ICommand? AddToCartCommand { get; set; }

        public ItemDTO? Item; //this (was) private to keep users from binding directly to information inside the item object

        //All of the info below Name/Description/ID is all set continuously as changes happen while performing add or edit.
        
        public string? Name //sets the name for the item through a public property
        {
            get
            {
                return Item?.Name ?? string.Empty;
            }
            set
            {
                if(Item != null)
                {
                    Item.Name = value;
                }
            }
        }
   
        public string? Description //sets the desc for the item through a public property
        {
            get
            {
                return Item?.Description ?? string.Empty;
            }
            set
            {
                if (Item != null)
                {
                    Item.Description = value;
                }
            }
        }
        
        public int ID //sets the ID for the item
        {
            get
            {
                return Item?.ID ?? 0;
            }
            set
            {
                if(Item != null)
                {  
                    Item.ID = value; 
                }
            }
        }

        public int Quantity
        {
            get
            {
                return Item?.Quantity ?? 0;
            }
            set
            {
                if (Item != null)
                {
                    Item.Quantity = value;
                }
            }
        }

        public decimal Price
        {
            get
            {
                return Item?.Price ?? 0m;
            }
            set
            {
                if (Item != null)
                {
                    Item.Price = value;
                }
            }
        }

        public bool IsBOGO
        {
            get
            {
                return Item?.IsBOGO ?? false; //assume that IsBOGO is false
            }
            set
            {
                if( Item != null)
                {
                    Item.IsBOGO = value;
                }
            }
        }

        public double Markdown
        {
            get
            {
                return Item?.Markdown ?? 0;
            }
            set
            {
                if (Item != null)
                {
                    Item.Markdown = value;
                }
            }
        }

        //This is only used to display that an item is BOGO from the shop screen (displays "BOGO" if BOGO, and empty string if not BOGO)
        public string IsBOGODisplay
        {
            get
            {
                if(IsBOGO == true)
                {
                    return "BOGO"; //display in the ui that this item is bogo
                }
                else
                {
                    return string.Empty; //not bogo
                }
            }
        }

        //This is used to display the markdown of the item (displays "% OFF" if there is a markdown on the item)
        public string IsMarkedDownDisplay
        {
            get
            {
                if (Markdown != 0)
                {
                    return $"({Markdown}% OFF)";
                }
                else
                {
                    return string.Empty; //no markdown
                }
            }
        }

        //in order to get to this function from xaml, you have to use the edit command
        //just as a side note, commands aren't being used in my project 2 so this can mostly be ignored
        private void ExecuteEdit(ItemViewModel? i)
        {
            if(i?.Item == null) //check if the item being passed in is null
            {
                return; //return if it is
            }
            Shell.Current.GoToAsync($"//Item?itemID={i.Item.ID}");
        }

        private void ExecuteDelete(int? id)
        {
            if(id == null)
            {
                return;
            }
            ItemServiceProxy.Current.Delete(id ?? 0);
        }
        
        private void ExecuteAddToCart(ItemViewModel? i) 
        {
            if (i?.Item == null)
            {
                return;
            }
            Shell.Current.GoToAsync($"//AddToCart?itemID={i.Item.ID}");
        }
        
        
        public void SetupCommands() //takes public products and binds them to a new instance of an object called command
        {
            //"EditCommand" is a property of type Command, a new instance of the command class is created. "i" represents
            //the argument passed in, i is attempted to be casted into the type ItemViewModel, if not possible the result will
            //be null. If not null, the cast is successful and ExecuteEdit is called
            //TLDR: if you want to edit an item you need the viewmodel to access the info
            EditCommand = new Command((i) => ExecuteEdit(i as ItemViewModel));

            AddToCartCommand = new Command((c) => ExecuteAddToCart(c as ItemViewModel));
        }
        
        public ItemViewModel() //default constructor for ItemViewModel
        {
            Item = new ItemDTO();
            
            SetupCommands(); //calls the function to set up the commands
        }

        public ItemViewModel(int id, string cloneThisIDInventory) //For adding an item to inventory
                                                         //This constructor takes an existing item, makes a clone of it
                                                         //and then sets the quantity to 1 (assuming the user by default
                                                         //would only want to buy one item. Need to make a clone because
                                                         //if you dont the item you return will be linked to the item
                                                         //in the inventory and AddToCart() function will act dumb.
        {
            ItemDTO existingItem = ItemServiceProxy.Current?.Items?.FirstOrDefault(i => i.ID == id); //we look through all of the existing items in the items list
            ItemDTO clonedItem = new ItemDTO(existingItem); //use the copy constructor in Item.cs to create a copy of the existing item
            clonedItem.Quantity = 1; //set the cloned item quantity to 1, assuming that the user would just want to add one item to their shopping cart
            Item = clonedItem; 
        }

        public ItemViewModel(int id, int cloneThisIDInventory) //For removing an item from shopping cart
        {
            int activeCart = ShoppingCartServiceProxy._SelectedShoppingCartID; //This is the currently active cart
            ItemDTO existingItem = ShoppingCartServiceProxy.Current?.Carts[activeCart - 1]?.Contents?.FirstOrDefault(i => i.ID == id); //we look through all of the existing items in the active cart
            ItemDTO clonedItem = new ItemDTO(existingItem); //use the copy constructor in Item.cs to create a copy of the existing item
            clonedItem.Quantity = 1; //set the cloned item quantity to 1, assuming that the user would just want to add one item to their shopping cart
            Item = clonedItem;

        }

        public ItemViewModel(int id) //constructor which takes in paramater id (ItemID) which was fetched by the query from the "itemID" value
                                     //and returns the item in the inventory at that ID
        {
            Item = ItemServiceProxy.Current?.Items?.FirstOrDefault(i => i.ID == id); //look through items and and find the first item with the same id
            if(Item == null)
            {
                //Potential issue here where two users could edit / delete at the same time and this would give you a blank name, however,
                //this sets the item.
                //This creates a new item with null values id the ID passed in isn't recognized
                Item = new ItemDTO();
            }
        }

        //This constructor takes in an item and sets the item inside this ViewModel to the item passed in
        public ItemViewModel(ItemDTO i) //paramaterized constructor for ItemViewModel
        {
            Item = i;
            SetupCommands(); //calls the function to set up the commands
        }

        public string? Display
        {
            get
            {
                return ToString();
            }
        }

        public async void Add() //Adds or update Item in inventory
        {
            if (Item != null)
            {
                Item = await ItemServiceProxy.Current.AddOrUpdate(Item);
            }
        }

 
        public void AddItemToCart()  //Adds or updates Item in cart
        {
            ShoppingCartServiceProxy.Current.AddItemToCart(Item);
        }

        public void RemoveItemFromCart()
        {
            ShoppingCartServiceProxy.Current.RemoveItemFromCart(Item);
        }
    }
}
