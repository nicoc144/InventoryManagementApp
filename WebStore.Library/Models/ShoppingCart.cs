using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebStore.Library.Services;

namespace WebStore.Library.Models
{
    public class ShoppingCart //represents a single shopping cart
    {
        
        public int ShoppingCartID { get; set; } //keeps track of whos shopping cart this is

        public string ShoppingCartName { get; set; } //name of shopping cart (for wishlists you might want to name your carts so you can keep track of them)

        public decimal ShoppingCartTotal {  get; set; } //this is the total price for the shopping cart

        public decimal ShoppingCartTotalAfterTax { get; set; }

        public List<Item>? Contents { get; set; } //this is a list of items

        public ShoppingCart()
        {
            Contents = new List<Item>();
        }

        public override string ToString()
        {
            return $"[{ShoppingCartID}] {ShoppingCartName} / ${ShoppingCartTotal} / ${ShoppingCartTotalAfterTax} / {Contents}";
        }
    }
}
