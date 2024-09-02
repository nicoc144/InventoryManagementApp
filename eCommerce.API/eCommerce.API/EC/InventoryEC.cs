﻿using eCommerce.API.Database;
using WebStore.Library.DTO;
using WebStore.Library.Models;

namespace eCommerce.API.EC
{
    public class InventoryEC //The enterprise contoller does all of the "heavy lifting," gets specificly used by the controller
    {

        public InventoryEC() {}

        public async Task<IEnumerable<ItemDTO>> Get()
        {
            //Database storage
            return new MSSQLContext().GetItems().Select(i => new ItemDTO(i));

            //File based storage
            //return Filebase.Current.Items.Select(i => new ItemDTO(i));
        }

        public async Task<ItemDTO> AddOrUpdate(ItemDTO i)
        {
            //Database storage
            return new ItemDTO(new MSSQLContext().AddItem(new Item(i)));

            //File based storage
            //return new ItemDTO(Filebase.Current.AddOrUpdate(new Item(i)));
        }

        public async Task<ItemDTO?> Delete(int id)
        {
            //Database storage
            return new ItemDTO(new MSSQLContext().DeleteItem(id));

            //File based storage
            //if (Filebase.Current.Items == null)
            //{
            //    return null;
            //}
            //return new ItemDTO(Filebase.Current.Delete(id));
        }
    }
}
