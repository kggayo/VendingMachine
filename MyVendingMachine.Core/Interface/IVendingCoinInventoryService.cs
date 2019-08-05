using MyVendingMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Interface
{
    public interface IVendingCoinInventoryService
    {
        IEnumerable<VendingCoin> PickCoinsForChange(decimal changeAmount);
        void AddCoins(IEnumerable<VendingCoin> coins);
        IEnumerable<VendingCoin> CreateInventorySnapshot();
        void RestoreInventorySnapshot(IList<VendingCoin> snapshot);
    }
}
