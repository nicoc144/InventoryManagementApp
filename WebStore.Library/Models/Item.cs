using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebStore.Library.DTO;

namespace WebStore.Library.Models
{
    public class Item //represents a single item
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public int ID { get; set; } //Id is set by the system, and can't be manually set
        public int Quantity { get; set; }
        public bool IsBOGO { get; set; } //t/f is the item buy one get one
        public double Markdown {  get; set; }

        //Un-used properties to demonstrate why DTOs are useful (ItemDTO doesn't have these properties because they're not used
        //in the program)
        public decimal TotalForThisItem { get; set; } //In order for BOGO to work proplerly, each item in the cart needs to keep
                                                      //track of it's personal total. This was used before database integration, no longer used.                      
        public string? ExpirationDate { get; set; }
        public string? SellByDate { get; set; }
        public string? AllergyWarning { get; set; }


        public override string ToString()
        {
            return $"[{ID}] {Name} / {Description} / ${Price} / {Quantity} / {IsBOGO} / {Markdown} / {TotalForThisItem}";
        }

        public Item() { }

        public Item(Item i) //copy constructor for product
        {
            Name = i.Name;
            Description = i.Description;
            Price = i.Price;
            ID = i.ID;
            Quantity = i.Quantity;
            IsBOGO = i.IsBOGO;
            Markdown = i.Markdown;
        }

        public Item(ItemDTO i)
        {
            Name = i.Name;
            Description = i.Description;
            Price = i.Price;
            ID = i.ID;
            Quantity = i.Quantity;
            IsBOGO = i.IsBOGO;
            Markdown = i.Markdown;
        }
    }
}
