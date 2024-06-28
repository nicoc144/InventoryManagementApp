using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WebStore.Library.Models
{
    public class Item //represents a single item
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int ID { get; set; } //Id is set by the system, and can't be manually set
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"[{ID}] {Name} / {Description} / ${Price} / {Quantity}";
        }
    }
}
