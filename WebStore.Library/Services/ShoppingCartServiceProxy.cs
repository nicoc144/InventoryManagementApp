using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
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
        public static int SelectedShoppingCartID;
        public static int _SelectedShoppingCartID
        { //Keeps track of the shopping cart being added to currently
            get
            {
                return SelectedShoppingCartID;
            }
            set
            {
                SelectedShoppingCartID = value;
            }
        } 

        //initilize a list of shopping carts
        private ShoppingCartServiceProxy()
        {
            var response = new WebRequestHandler().Get("/Shop").Result;
            carts = JsonConvert.DeserializeObject<List<ShoppingCartDTO>>(response);
            
            if(carts.Count != 0) //This should always be true, since there should be a default cart
            {
                SelectedShoppingCartID = 1;
            }
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

        public async Task<ShoppingCartDTO> AddOrUpdateCartDetails(ShoppingCartDTO? cart)
        {
            var result = await new WebRequestHandler().Post("/Shop", cart);
            var cartToAddOrUpdate = JsonConvert.DeserializeObject<ShoppingCartDTO>(result);
            return cartToAddOrUpdate;
        }

        public async Task<ShoppingCartDTO> DeleteCart(int id) //deletes an item based on the id passed in
        {
            var result = await new WebRequestHandler().Delete($"/Shop/{id}");
            var cartToDelete = JsonConvert.DeserializeObject<ShoppingCartDTO>(result);
            return cartToDelete;
        }

        public async Task<ItemDTO> AddItemToCart(ItemDTO? item)
        {
            var result = await new WebRequestHandler().Post($"/Shop/{SelectedShoppingCartID}", item);
            var itemToAdd = JsonConvert.DeserializeObject<ItemDTO>(result);
            return itemToAdd;
        }

        public async Task<ItemDTO> RemoveItemFromCart(ItemDTO? item)
        {
            var result = await new WebRequestHandler().Post($"/Shop/DeleteCartItem/{SelectedShoppingCartID}", item);
            var itemToAdd = JsonConvert.DeserializeObject<ItemDTO>(result);
            return itemToAdd;
        }
    }
}
