using MyVendingMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Interface
{
    public interface IVendingMachineService
    {
        IEnumerable<VendingItem> GetVendingItems();
        TransactionResult WithdrawVendingItem(VendingItem item, IEnumerable<VendingCoin> insertedCoins);
    }
}
