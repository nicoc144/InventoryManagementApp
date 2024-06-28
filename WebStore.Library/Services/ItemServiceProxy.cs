using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.Models;

namespace WebStore.Library.Services
{
    public class ItemServiceProxy //represents a list of items
    {
        private ItemServiceProxy()
        { //private constructor, only a function inside the class can create an instance of itself
            items = new List<Item> { new Item { Name = "Bread", ID = 1, Description = "DefaultDesc", Price = 2.32m, Quantity = 23},
                new Item { Name = "Milk", ID = 2, Description = "DefaultDesc" , Price = 1.28m, Quantity = 12} };
        }

        //use constructor below this if u want to initilize an empty list
        
        //private ItemServiceProxy()
        //{
        //    items = new List<Item>();
        //}

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
        private List<Item>? items; //list that holds item objects
        public ReadOnlyCollection<Item>? Items //provides read only access to the private items list shown in the previous line of code
        {
            get
            {
                return items?.AsReadOnly();
            }
        }

        //===FUNCTIONALITY===

        public int LastID
        {
            get
            {
                if (items?.Any() ?? false)
                {
                    return items?.Select(i => i.ID)?.Max() ?? 0;
                }
                return 0;
            }
        }

        public Item? AddOrUpdate(Item? item)
        {
            if (items == null) //check that the singleton list is null, if it is return null to avoid runtime error
            {
                return null;
            }

            bool isAdd = false; //assume that this function is for an update first

            if (item.ID == 0) //check if the item you're adding has an id of 0
            {
                item.ID = LastID + 1; //set the item id to the last id + 1, every item in the list needs a unique id
                isAdd = true; //this is an add, since the item's id was initially 0
            }

            if (isAdd) //if its an add
            {
                items.Add(item); //add the item to the items list
            }

            return item;
        }

        public void Delete(int id) //deletes an item based on the id passed in
        {
            if (items == null)
            {
                return;
            }

            var itemToDelete = items.FirstOrDefault(i => i.ID == id); //find the first item in the list that has the same id as the one passed in

            if (itemToDelete != null) //if the item isn't null, delete it
            {
                items.Remove(itemToDelete);
            }
        }

       

    }
}
