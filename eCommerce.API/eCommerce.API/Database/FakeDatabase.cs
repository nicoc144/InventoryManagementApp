using WebStore.Library.Models;

namespace eCommerce.API.Database
{
    public static class FakeDatabase
    {
        public static int LastID
        {
            get
            {
                if (Items?.Any() ?? false)
                {
                    return Items?.Select(i => i.ID)?.Max() ?? 0;
                }
                return 0;
            }
        }
        public static List<Item> Items { get; } = new List<Item> { new Item { Name = "MacBook Pro", ID = 1, Description = "Apple 13in Macbook 512GB SSD Silver", Price = 1599.00m, Quantity = 23, IsBOGO = false},
                new Item { Name = "I Phone 15 Pro", ID = 2, Description = "Apple I Phone 15 Pro Space Grey" , Price = 849.00m, Quantity = 12, IsBOGO = false},
                new Item { Name = "Airpods Pro", ID = 3, Description = "Apple Airpods Pro 2" , Price = 124.00m, Quantity = 54, IsBOGO = false}
            };
    }
}
