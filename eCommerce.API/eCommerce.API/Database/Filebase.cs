using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Library.Models;

namespace eCommerce.API.Database
{
    public class Filebase
    {
        private string _root;
        private static Filebase _instance;


        public static Filebase Current //singleton
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase() //on server
        {
            _root = @"C:\temp\Items"; //@ symbol means you can't modity anything inside quotation marks
                                      //This is where the files are stored
        }

        public int NextID //get the next id of the item
        {
            get
            {
                if (!Items.Any())
                {
                    return 1;
                }
                return Items.Select(i => i.ID).Max() + 1;
            }
        }

        public Item AddOrUpdate(Item item)
        {
            //set up a new Id if one doesn't already exist
            if(item.ID <= 0)
            {
                item.ID = NextID;
            }

            //every product has it's own file (file systems are better at handling smaller files)
            string path = $"{_root}\\{item.ID}.json";

            //if the item has been previously persisted
            if(File.Exists(path))
            {
                //delete it
                File.Delete(path);
            }

            //Write the file
            //Could cause issues if the item's name is really long, that long name would have to be seralized and written to disk.
            //Or if information is being sent to the file and somehow error checking code for the disk is triggered, the disk could stop
            //responding and the code will fail (potentially even silently).
            File.WriteAllText(path, JsonConvert.SerializeObject(item));

            //return the item, which now has an id
            return item;
        }

        public List<Item> Items
        {
            get
            {
                var root = new DirectoryInfo(_root);
                var itemList = new List<Item>();
                foreach (var appFile in root.GetFiles())
                {
                    var myItem = JsonConvert.DeserializeObject<Item>(File.ReadAllText(appFile.FullName));
                    if(myItem != null)
                    {
                        itemList.Add(myItem);
                    }
                }
                return itemList;
            }
        }

        public Item Delete(int id)
        {

            Item returnItem = Items.FirstOrDefault(i => i.ID == id); //look for the item in the items list with same id

            //every product has it's own file (file systems are better at handling smaller files)
            string path = $"{_root}\\{id}.json";

            //if the item has been previously persisted
            if (File.Exists(path))
            {
                //delete it
                File.Delete(path);
            }

            return returnItem; //return the item from the items list
        }
    }
}
