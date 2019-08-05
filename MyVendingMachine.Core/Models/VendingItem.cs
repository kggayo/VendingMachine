using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Models
{
    public class VendingItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
