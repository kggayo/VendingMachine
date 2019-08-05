using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyVendingMachine.Core.Models
{
    public enum CoinDenomination
    {
        [EnumMember(Value = "CENT(S)")]
        CENT,
        [EnumMember(Value = "EURO")]
        EURO
    }
}
