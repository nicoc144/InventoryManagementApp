using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebStore.Library.Models;

namespace WebStore.Library.DTO
{
    public class ShoppingCartDTO
    {
        public int ShoppingCartID { get; set; } //keeps track of whos shopping cart this is

        public string ShoppingCartName { get; set; } //name of shopping cart (for wishlists you might want to name your carts so you can keep track of them)
        public int UserID { get; set; }

        public decimal ShoppingCartTotal { get; set; } //this is the total price for the shopping cart

        public decimal ShoppingCartTotalAfterTax { get; set; }

        public double ShoppingCartTax {  get; set; }

        public List<ItemDTO>? Contents { get; set; } //this is a list of items

        public ShoppingCartDTO()
        {
            Contents = new List<ItemDTO>();
        }

        public ShoppingCartDTO(ShoppingCart s) //Maps ShoppingCart into ShoppingCartDTO
        {
            ShoppingCartID = s.ShoppingCartID;
            ShoppingCartName = s.ShoppingCartName;
            UserID = s.UserID;
            ShoppingCartTotal = s.ShoppingCartTotal;
            ShoppingCartTotalAfterTax = s.ShoppingCartTotalAfterTax;
            Contents = s.Contents;
            ShoppingCartTax = s.ShoppingCartTax;
        }
    }
}
