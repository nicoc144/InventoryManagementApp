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
            var carts = new MSSQLContext().GetCarts();
            decimal total = 0;
            return carts.Select(cart =>
            {
                foreach(var item in cart.Contents)
                {
                    if(item.IsBOGO)
                    {
                        if(item.Quantity % 2 == 0)
                        {
                            total = total + (item.Price * (item.Quantity / 2));
                        }
                        else
                        {
                            total = total + (item.Price * ((item.Quantity / 2)+1));
                        }
                    }
                    else
                    {
                        total = total + (item.Price * item.Quantity);
                    }
                }

                //I'm chosing to calculate the tax here since having this on the client side could cause vulnerabilities.
                //This data is not saved in the database, however it's saved in the JSON blob for shop, and is calculated everytime
                //the Get() is called.
                cart.ShoppingCartTax = GetShoppingCartTax().Result;

                decimal totalAfterTax = total * (Decimal)(cart.ShoppingCartTax / 100); //Calculate the tax in dollars

                totalAfterTax = totalAfterTax + total; //Calculate the total after tax by adding the tax in dollars to the total

                var shoppingCart = new ShoppingCartDTO(cart) //Set the values
                {
                    ShoppingCartTotal = Math.Round(total,2),
                    ShoppingCartTax = cart.ShoppingCartTax,
                    ShoppingCartTotalAfterTax = Math.Round(totalAfterTax,2)
                };
                return shoppingCart;
            });
        }

        public async Task<double> GetShoppingCartTax() //In this function you should call some API that gives you the tax rate.
                                                       //Right now this is just a simulation.
        {
            double CurrentTax = 0;
            Random random = new Random();
            CurrentTax = Math.Round((random.NextDouble() * (7-5)+5), 2); //Multiply the difference of 7-5 + 5 for a value between 7 and 5
            return CurrentTax;
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
