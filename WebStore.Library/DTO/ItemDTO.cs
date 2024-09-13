using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Library.Models;

namespace WebStore.Library.DTO
{
    public class ItemDTO //DTOs transfer only absolutely neccescary data for a specific task to reduce overhead
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public int ID { get; set; } //Id is set by the system, and can't be manually set
        public int Quantity { get; set; }
        public bool IsBOGO { get; set; } //t/f is the item buy one get one
        public double Markdown { get; set; }
        
        public ItemDTO(Item i) //Maps Item into ItemDTO
        {
            Name = i.Name;
            Description = i.Description;
            Price = i.Price;
            ID = i.ID;
            Quantity = i.Quantity;
            IsBOGO = i.IsBOGO;
            Markdown = i.Markdown;
             
        }

        public ItemDTO(ItemDTO i) //copy constructor for product
        {
            Name = i.Name;
            Description = i.Description;
            Price = i.Price;
            ID = i.ID;
            Quantity = i.Quantity;
            IsBOGO = i.IsBOGO;
            Markdown = i.Markdown;
        }

        public ItemDTO()
        { 
        
        }

    }
}
