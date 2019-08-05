using MyVendingMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Data
{
    public static class VendingItemRepository
    {
        public static IList<VendingItem> VendingItems { get; set; }

        static VendingItemRepository()
        {
            VendingItems = new List<VendingItem>() {
                new VendingItem(){ Name = "Tea", Price = 1.30M, Quantity = 10 },
                new VendingItem(){ Name = "Espresso", Price = 1.80M, Quantity = 20 },
                new VendingItem(){ Name = "Juice", Price = 1.80M, Quantity = 20 },
                new VendingItem(){ Name = "Chicken Soup", Price = 1.80M, Quantity = 15 },
            };
        }
    }
}
