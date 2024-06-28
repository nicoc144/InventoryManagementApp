using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.Models;

namespace WebStore.Library.Services
{
    public class ShoppingCartService //represents a list of shopping carts
    {

        //initilize a list of shopping carts
        private ShoppingCartService()
        {
            carts = new ReadOnlyCollection<ShoppingCart>(
                new List<ShoppingCart> //create a new list of shopping carts
                {
                    new ShoppingCart //initilize values for each shopping cart
                    {
                        ID = 1, //id doesn't really matter right now, can add logic to change this later on
                        ShoppingCartTotal = 0m, //initial total is 0
                        Contents = new List<Item>() //create a new list of items, which is the contents of a single shopping cart
                    }
                }
            );  
            

        }

        private static ShoppingCartService? instance;
        private static object instanceLock = new object();

        public ReadOnlyCollection<ShoppingCart> carts; //read only collection of shopping carts

        public ShoppingCart Cart //right now this only allows for access of the first cart
        {
            get
            {
                if (carts == null || !carts.Any()) //if there are no carts in the list of shopping carts
                {
                    return new ShoppingCart(); //return a new shopping cart
                }
                return carts?.FirstOrDefault() ?? new ShoppingCart(); //if carts is not null, return first or default
            }
        }
        
        public static ShoppingCartService Current
        {
            get
            {
                lock (instanceLock) //only one thread can enter this section of code at a time
                {
                    if (instance == null) //if there isn't an instance
                    {
                        instance = new ShoppingCartService(); //create an instance of shoppingcartservice, ensures that there is only one instance of this class
                    }
                    return instance;
                }
            }
        }
        

        /*
        public ShoppingCart AddOrUpdate(ShoppingCart c) //creating a cart in a list of carts
        {

        }
        */

        public void AddToCart(Item newItem) //adding a product in a cart, updates quantity automatically if the item is already in the cart
        {
            if (Cart == null || Cart.Contents == null) 
            {
                return;
            }

            //create varaible existingItem, first check that cart and contents are not null, then loop through all of the existing items
            //looking for the first item that matches the id. If item cant be found return default (or null). "existingItems" is just a placeholder name
            //for Contents in shopping cart
            var existingItem = Cart?.Contents?.FirstOrDefault(existingItems => existingItems.ID == newItem.ID);

            //remove the quantity from the inventory
            var inventoryItem = ItemServiceProxy.Current.Items.FirstOrDefault(invItem => invItem.ID == newItem.ID);

            if (inventoryItem == null)
            { //if the inventory item doesnt exist
                return;
            }
            inventoryItem.Quantity -= newItem.Quantity;

            //CALCULATE THE TOTAL
            Cart.ShoppingCartTotal += newItem.Quantity * newItem.Price;

            if (existingItem != null)
            { //check if the item already exists in the shopping cart
                //update (item already exists), adding the new quantity with the existing quantity
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                //add (item does not already exist)
                //should also set a new id but not sure
                Cart.Contents.Add(newItem);
            }
        }
    }
}
