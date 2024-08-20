using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.Models;
using Newtonsoft;
using Newtonsoft.Json;
using WebStore.Library.Utilities;
using WebStore.Library.DTO;

namespace WebStore.Library.Services
{
    public class ItemServiceProxy //represents a list of items
    {

        private ItemServiceProxy() //private constructor, only a function inside the class can create an instance of itself
        {
            //make web call to get the items list from the inventory server 
            var response = new WebRequestHandler().Get("/Inventory").Result;
            items = JsonConvert.DeserializeObject<List<ItemDTO>>(response);

        }

        private static ItemServiceProxy? instance; //backing field, question mark makes this statement nullable, which ensures that
                                                   //the instance is null if instance hasn't been set before
        private static object instanceLock = new object(); //used for the lock, need lock to support multithreading
        public static ItemServiceProxy Current //singleton access property
        {
            get //retrieves the value "instance"
            {
                lock (instanceLock) //lock ensures that the code below is executed one thread at a time
                {
                    if (instance == null) //check if the instance is null
                    {
                        instance = new ItemServiceProxy(); //if the instance is null, create a new one
                    }
                }
                return instance;
            }
        }
        private List<ItemDTO>? items; //list that holds item objects
        public ReadOnlyCollection<ItemDTO>? Items //provides read only access to the private items list shown in the previous line of code
        {
            get
            {
                return items?.AsReadOnly();
            }
        }

        //get web call, gets the list stored in the json blob
        public async Task<IEnumerable<ItemDTO>> Get() //async is a keyword that says this method is asyncronous
        {
            var result = await new WebRequestHandler().Get("/Inventory"); //Make web call to get the items list from the server
                                                                          //The "await" keyword specifies to not move on until the webrequesthandler successfully retrieves the full inventory list (ie this line of code is done executing)
            var deserializedResult = JsonConvert.DeserializeObject<List<ItemDTO>>(result); //convert json blob into list of ItemDTO
            items = deserializedResult?.ToList() ?? new List<ItemDTO>(); //return empty if connection to data center is lost
            return items;
        }

        //addorupdate web call, adds or updates using "post" depending on if the item passed in has an id or not, it will either be updated or a new id will be created for a new item
        public async Task<ItemDTO> AddOrUpdate(ItemDTO? item)
        {
            var result = await new WebRequestHandler().Post("/Inventory", item);
            var itemToAddOrUpdate = JsonConvert.DeserializeObject<ItemDTO>(result);
            return itemToAddOrUpdate;
        }

        //delete web call, deletes item from json blob with specified id
        public async Task<ItemDTO> Delete(int id) //deletes an item based on the id passed in
        {
            var result = await new WebRequestHandler().Delete($"/{id}");
            var itemToDelete = JsonConvert.DeserializeObject<ItemDTO>(result);
            return itemToDelete;
        }

       

    }
}
