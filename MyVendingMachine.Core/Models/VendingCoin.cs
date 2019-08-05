using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyVendingMachine.Core.Models
{
    public class VendingCoin : ICloneable
    {
        public int Value { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public CoinDenomination CoinDenomination { get; set; }
        public int Quantity { get; set; }

        public decimal Amount {
            get {
                if (CoinDenomination == CoinDenomination.CENT)
                    return (((decimal)(Value * Quantity)) / 100M);
                else
                    return (decimal)(Value * Quantity);
            }
        }

        public object Clone()
        {
            return new VendingCoin() { Value = this.Value, CoinDenomination = this.CoinDenomination, Quantity = this.Quantity };
        }
    }
}
