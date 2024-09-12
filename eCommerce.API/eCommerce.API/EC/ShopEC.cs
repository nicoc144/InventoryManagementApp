using eCommerce.API.Database;
using WebStore.Library.DTO;
using WebStore.Library.Models;

namespace eCommerce.API.EC
{
    public class ShopEC 
    {
        public ShopEC() { }
        public async Task<IEnumerable<ShoppingCartDTO>> Get()
        {
            return new MSSQLContext().GetCarts().Select(i => new ShoppingCartDTO(i));
        }

        public async Task<ShoppingCartDTO> AddOrUpdate(ShoppingCartDTO c)
        {
            if (c.ShoppingCartID != 0) //If this is an update
            {
                if (new MSSQLContext().GetCarts().FirstOrDefault(cart => cart.ShoppingCartID == c.ShoppingCartID) == null)
                {
                    return null; //return null if the item you're trying to delete doesn't exist in the list
                }
            }
            return new ShoppingCartDTO(new MSSQLContext().AddCart(new ShoppingCart(c)));
        }

        public async Task<ShoppingCartDTO?> Delete(int id)
        {
            if (new MSSQLContext().GetCarts().FirstOrDefault(cart => cart.ShoppingCartID == id) == null || id == 1) //Check that the id exists, and that its not the default cart
            {
                return null; //Return null if the item you're trying to delete doesn't exist in the list or is default cart
            }
            return new ShoppingCartDTO(new MSSQLContext().DeleteCart(id));
        }

        public async Task<ItemDTO> AddItemToCart(ItemDTO i, int activeCartID)
        {
            Item ItemInInventory = new MSSQLContext().GetItems().FirstOrDefault(item => item.ID == i.ID); //Look for the item in the inventory
            if (ItemInInventory == null || i == null || activeCartID == 0)
            {
                return null;
            }
           
            ItemDTO ItemInCart = new ItemDTO(new MSSQLContext().AddItemToCart(new Item(i), activeCartID)); //This is the item being added, along with desired quantity
            int UpdatedInvQuantity = ItemInInventory.Quantity - i.Quantity; //This is the new inventory quantity
            
            if(UpdatedInvQuantity < 0) //Check that the new quantity is not negative
            {
                return null;
            }
            ItemInInventory.Quantity = UpdatedInvQuantity; //Set the new quantity for the item in inventory
            new MSSQLContext().AddItem(new Item(ItemInInventory)); //Update the item in the inventory with the reduced quant

            return ItemInCart;
        }
    }
}
