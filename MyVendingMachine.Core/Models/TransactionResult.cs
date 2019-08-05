using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Models
{
    public class TransactionResult
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<VendingCoin> ReturnedCoins { get; set; }
        public decimal ReturnedAmount { get; set; }
    }
}
