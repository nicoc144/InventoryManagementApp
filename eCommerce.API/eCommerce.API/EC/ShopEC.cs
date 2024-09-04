using eCommerce.API.Database;
using WebStore.Library.DTO;
using WebStore.Library.Models;

namespace eCommerce.API.EC
{
    public class ShopEC //The enterprise contoller does all of the "heavy lifting," gets specificly used by the controller
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
                    return null; //Return null if the item you're trying to delete doesn't exist in the list
                }
            }
            return new ShoppingCartDTO(new MSSQLContext().AddCart(new ShoppingCart(c)));

        }
    }
}
