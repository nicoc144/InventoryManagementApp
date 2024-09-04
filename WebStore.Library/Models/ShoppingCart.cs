using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebStore.Library.DTO;
using WebStore.Library.Services;

namespace WebStore.Library.Models
{
    public class ShoppingCart //represents a single shopping cart
    {
        
        public int ShoppingCartID { get; set; } //keeps track of whos shopping cart this is

        public string ShoppingCartName { get; set; } //name of shopping cart (for wishlists you might want to name your carts so you can keep track of them)

        public int UserID { get; set; }

        public decimal ShoppingCartTotal {  get; set; } //this is the total price for the shopping cart

        public decimal ShoppingCartTotalAfterTax { get; set; }

        //Un-used properties to demonstrate why DTOs are useful (ShoppingCartDTO doesn't have these properties because they're not used
        //in the program)
        public decimal DonateToCharityAmmount { get; set; }
        public int DiscountCode { get; set; }

        public List<ItemDTO>? Contents { get; set; } //this is a list of items

        public ShoppingCart()
        {
            Contents = new List<ItemDTO>();
        }

        public ShoppingCart(ShoppingCartDTO s)
        {
            ShoppingCartID = s.ShoppingCartID;
            ShoppingCartName = s.ShoppingCartName;
            UserID = s.UserID;
            ShoppingCartTotal = s.ShoppingCartTotal;
            ShoppingCartTotalAfterTax = s.ShoppingCartTotalAfterTax;
            Contents = s.Contents;
        }

        public override string ToString()
        {
            return $"[{ShoppingCartID}] {ShoppingCartName} /USR[{UserID}] / ${ShoppingCartTotal} / ${ShoppingCartTotalAfterTax} / {Contents}";
        }
    }
}
