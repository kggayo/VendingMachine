using MyVendingMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Data
{
    public static class VendingCoinRepository
    {
        public static IList<VendingCoin> VendingCoins { get; set; }

        static VendingCoinRepository()
        {
            VendingCoins = new List<VendingCoin>()
            {
                new VendingCoin(){ Value = 1, Quantity = 100 },
                new VendingCoin(){ Value = .10M, Quantity = 100 },
                new VendingCoin(){ Value = .20M, Quantity = 100 },
                new VendingCoin(){ Value = .50M, Quantity = 100 },
            };
        }
    }
}
