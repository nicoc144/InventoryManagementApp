using eCommerce.API.Database;
using WebStore.Library.DTO;
using WebStore.Library.Models;

namespace eCommerce.API.EC
{
    public class InventoryEC //enterprise controller for inventory
    {

        public InventoryEC()
        {
            

        }

        public async Task<IEnumerable<ItemDTO>> Get()
        {
            return Filebase.Current.Items.Take(100).Select(i => new ItemDTO(i));
        }

        public async Task<ItemDTO> AddOrUpdate(ItemDTO i)
        {
            //if (i == null) //check that the singleton list is null, if it is return null to avoid runtime error
            //{
            //    return null;
            //}

            //bool isAdd = false; //assume that this function is for an update first

            //if (i.ID == 0) //check if the item you're adding has an id of 0
            //{
            //    i.ID = Filebase.Current.NextID; //set the item id to the last id + 1, every item in the list needs a unique id
            //    isAdd = true; //this is an add, since the item's id was initially 0
            //}

            //if (isAdd) //if its an add
            //{
            //    Filebase.Current.Items.Add(new Item(i)); //add the item to the items list
            //}
            //else
            //{
            //    var itemToUpdate = Filebase.Current.Items.FirstOrDefault(a => a.ID == i.ID);
            //    if (itemToUpdate != null)
            //    {
            //        var index = Filebase.Current.Items.IndexOf(itemToUpdate); //set index to the item to update
            //        Filebase.Current.Items.RemoveAt(index); //remove at the index
            //        itemToUpdate = new Item(i);
            //        Filebase.Current.Items.Insert(index, itemToUpdate); //insert the updated item at the index
            //    }
            //}

            return new ItemDTO(Filebase.Current.AddOrUpdate(new Item(i)));
        }

        public async Task<ItemDTO?> Delete(int id)
        {
            if (Filebase.Current.Items == null)
            {
                return null;
            }

            return new ItemDTO(Filebase.Current.Delete(id));

            //var itemToDelete = Filebase.Current.Items.FirstOrDefault(i => i.ID == id); //find the first item in the list that has the same id as the one passed in
            //if (itemToDelete != null) //if the item isn't null, delete it
            //{
            //    Filebase.Current.Items.Remove(itemToDelete);
            //}
            //return new ItemDTO(itemToDelete);
        }
    }
}
