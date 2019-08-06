using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Models
{
    public class VendingCoin : ICloneable
    {
        public decimal Value { get; set; }
        
        public string CoinDenomination
        {
            get
            {
                if (Math.Truncate(Value) == 0)
                    return "CENT(S)";
                else
                    return "EURO";
            }
        }

        public int Quantity { get; set; }

        public decimal Amount
        {
            get
            {
                return (decimal)(Value * Quantity);
            }
        }

        public object Clone()
        {
            return new VendingCoin() { Value = this.Value, Quantity = this.Quantity };
        }
    }
}
