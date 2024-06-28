using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.Services;

namespace WebStore.Library.Models
{
    public class ShoppingCart //represents a single shopping cart
    {
        public int ID { get; set; } //keeps track of whos shopping cart this is

        public decimal ShoppingCartTotal {  get; set; } //this is the total price for the shopping cart

        public List<Item>? Contents { get; set; }
    }
}
