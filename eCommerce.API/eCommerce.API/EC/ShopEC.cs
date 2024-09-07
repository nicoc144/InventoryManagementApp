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

        public async Task<IEnumerable<ItemDTO>> GetCartItems(int activeCartID)
        {
            return new MSSQLContext().GetItemsForCart(activeCartID).Select(i => new ItemDTO(i));
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

        //public async Task<ItemDTO> AddItemToCart()
        //{

        //}
    }
}
