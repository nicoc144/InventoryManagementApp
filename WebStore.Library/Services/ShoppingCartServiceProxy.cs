using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.DTO;
using WebStore.Library.Models;
using WebStore.Library.Utilities;

namespace WebStore.Library.Services
{
    public class ShoppingCartServiceProxy //represents a list of shopping carts
    {
        public static int SelectedShoppingCartID { get; private set; } //keeps track of the shopping cart being added to currently

        public static double TaxForAllItems { get; private set; }

        //initilize a list of shopping carts
        private ShoppingCartServiceProxy()
        {
            var response = new WebRequestHandler().Get("/Shop").Result;
            carts = JsonConvert.DeserializeObject<List<ShoppingCartDTO>>(response);
        }

        private static ShoppingCartServiceProxy? instance;

        private static object instanceLock = new object();

        public static ShoppingCartServiceProxy Current
        {
            get
            {
                lock (instanceLock) //only one thread can enter this section of code at a time
                {
                    if (instance == null) //if there isn't an instance
                    {
                        instance = new ShoppingCartServiceProxy(); //create an instance of shoppingcartservice, ensures that there is only one instance of this class
                    }
                }
                return instance;
            }
        }

        private List<ShoppingCartDTO> carts; //read only collection of shopping carts


        public ReadOnlyCollection<ShoppingCartDTO> Carts
        {
            get
            {
                return carts?.AsReadOnly();
            }
        }

        public async Task<IEnumerable<ShoppingCartDTO>> Get() //async is a keyword that says this method is asyncronous
        {
            var result = await new WebRequestHandler().Get("/Shop"); //Make web call to get the items list from the server
                                                                          //The "await" keyword specifies to not move on until the webrequesthandler successfully retrieves the full inventory list (ie this line of code is done executing)
            var deserializedResult = JsonConvert.DeserializeObject<List<ShoppingCartDTO>>(result); //convert json blob into list of ItemDTO
            carts = deserializedResult?.ToList() ?? new List<ShoppingCartDTO>(); //return empty if connection to data center is lost
            return carts;
        }

        public async Task<ShoppingCartDTO> AddOrUpdate(ShoppingCartDTO? cart)
        {
            var result = await new WebRequestHandler().Post("/Shop", cart);
            var cartToAddOrUpdate = JsonConvert.DeserializeObject<ShoppingCartDTO>(result);
            return cartToAddOrUpdate;
        }

        public static void SetTaxForAllItems(double d)
        {
            TaxForAllItems = d;
        }

        public ShoppingCartDTO? AddOrUpdateCartDetails(ShoppingCartDTO? c) //creating a cart in a list of carts
        {
            if(carts == null)
            {
                return null; //return null if carts is null
            }

            bool isAdd = false; //assume youre editing a cart

            if(c.ShoppingCartID == 0) //check if the shopping cart id exists
            {
                c.ShoppingCartID = LastID + 1; //if id doesn't exist, assign a new id
                isAdd = true; //this is an add
            }

            if(isAdd)
            {
                carts.Add(c); //had to change carts to List from ReadOnlyCollection for this to work, but this adds c to the carts list
            }

            return c; //return the cart
        }

        public int LastID //find the last id of the shopping carts
        {
            get
            {
                if (carts?.Any() ?? false)
                {
                    return carts?.Select(c => c.ShoppingCartID)?.Max() ?? 0;
                }
                return 0;
            }
        }

        public static void SetCurrentShoppingCart(int id)
        {
            if (id == 0)
            {
                return;
            }
            SelectedShoppingCartID = id;
        }


        public void AddToCart(ItemDTO newItem) //adding a product in a cart, updates quantity automatically if the item is already in the cart
        {
            ShoppingCartDTO selectedCart = ShoppingCartServiceProxy.Current.Carts.FirstOrDefault(c => c.ShoppingCartID == SelectedShoppingCartID);

            if (selectedCart == null || selectedCart.Contents == null)
            {
                return;
            }

            //create varaible existingItem, first check that cart and contents are not null, then loop through all of the existing items
            //looking for the first item that matches the id. If item cant be found return default (or null). "existingItems" is just a placeholder name
            //for Contents in shopping cart
            var existingItem = selectedCart?.Contents?.FirstOrDefault(existingItems => existingItems.ID == newItem.ID);
                    
            //remove the quantity from the inventory
            var inventoryItem = ItemServiceProxy.Current.Items.FirstOrDefault(invItem => invItem.ID == newItem.ID);

            if (inventoryItem == null || existingItem?.Quantity > inventoryItem.Quantity || newItem?.Quantity > inventoryItem.Quantity)
            {
                return; //return nothing if the inventory item is null or if the requested quantity is greater than the inventory quantity
            }
            inventoryItem.Quantity -= newItem.Quantity; //reduce inventory quantity

            if (existingItem != null) //check if the item already exists in the shopping cart
            {
                existingItem.Quantity += newItem.Quantity; //update the quntity for this existing item

                existingItem.IsBOGO = newItem.IsBOGO; //update the "IsBOGO" value to the newItem's value for IsBOGO

                existingItem.Markdown = newItem.Markdown; //updtae the "Markdown" value to the newItem's value

                if (existingItem.IsBOGO == true) //existing item is BOGO
                {
                    if (existingItem.Quantity % 2 == 0) //check if the item quantity youre adding is even
                    {
                        //calculate the new total for this item, if you add a bogo item one by one this will calculate the correct total for the item
                        existingItem.TotalForThisItem = existingItem.Quantity * (newItem.Price / 2);
                    }
                    else
                    {
                        //same deal as the code under the if statement, just for odd quantity
                        existingItem.TotalForThisItem = (existingItem.Quantity - 1) * (newItem.Price / 2);
                        existingItem.TotalForThisItem += newItem.Price;
                    }
                }
                else //existing item isn't BOGO
                {
                    existingItem.TotalForThisItem = existingItem.Quantity * existingItem.Price;
                }
            }
            else
            {
                if (newItem.IsBOGO == true) //new item is BOGO
                {
                    if (newItem.Quantity % 2 == 0) //check if the item quantity youre adding is even
                    {
                        //calculate the total for new bogo item with even quant
                        newItem.TotalForThisItem = newItem.Quantity * (newItem.Price / 2);
                        selectedCart.Contents.Add(newItem);
                    }
                    else
                    {
                        //calculate the total for new item with odd quant
                        newItem.TotalForThisItem = (newItem.Quantity - 1) * (newItem.Price / 2);
                        newItem.TotalForThisItem += newItem.Price;
                        selectedCart.Contents.Add(newItem);
                    }
                }
                else //new item isn't BOGO
                {
                    newItem.TotalForThisItem = newItem.Quantity * newItem.Price; //calculate total for this item being added to cart
                    selectedCart.Contents.Add(newItem); //add (item does not already exist)
                }
            }
            
            Decimal TempTotal = 0m; //Finds the total after item markdown

            for (int i = 0; i < selectedCart.Contents.Count(); i++)
            {
                TempTotal += selectedCart.Contents[i].TotalForThisItem - (selectedCart.Contents[i].TotalForThisItem * ((Decimal)selectedCart.Contents[i].Markdown / 100));
            }

            selectedCart.ShoppingCartTotal = Math.Round(TempTotal, 2);

            //Loop through all of the contents of the cart and add their individual totals
            //Cart.ShoppingCartTotal = Cart?.Contents?.Sum(c => c.TotalForThisItem) ?? 0m;

            //Set the total after tax to the same value (to be subtracted from later)
            selectedCart.ShoppingCartTotalAfterTax = selectedCart.ShoppingCartTotal;

            //Calculate the tax ammount in dollar amount
            Decimal TaxInDollars = Math.Round(selectedCart.ShoppingCartTotal * ((Decimal)TaxForAllItems/ 100), 2);

            //Subtract the tax from the shopping cart total and set it to ShoppingCartTotalAfterTax
            selectedCart.ShoppingCartTotalAfterTax += TaxInDollars;
        }
    }
}
