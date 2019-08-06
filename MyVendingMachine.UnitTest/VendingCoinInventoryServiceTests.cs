using MyVendingMachine.Core.Data;
using System;
using Xunit;
using System.Linq;
using MyVendingMachine.Core.Models;
using System.Collections.Generic;
using MyVendingMachine.Service.Services;

namespace MyVendingMachine.UnitTest
{
    [Collection("Sequential")]
    public class VendingCoinInventoryServiceTests
    {
        [Fact]
        public void PickCoinsForChange_ValidateCentChangeAmount_ReturnsCorrectAmount()
        {
            VendingCoinInventoryService vis = new VendingCoinInventoryService();

            var result = vis.PickCoinsForChange(.30M);
            var changeAmount = result.Sum(s => s.Amount);

            Assert.Equal(.30M, changeAmount);
        }

        [Fact]
        public void PickCoinsForChange_ValidateEuroCentChangeAmount_ReturnsCorrectAmount()
        {
            VendingCoinInventoryService vis = new VendingCoinInventoryService();

            var result = vis.PickCoinsForChange(1.30M);
            var changeAmount = result.Sum(s => s.Amount);

            Assert.Equal(1.30M, changeAmount);
        }

        [Fact]
        public void PickCoinsForChange_NoEuroLeft_ReturnsChangeAsCents()
        {
            VendingCoinInventoryService vis = new VendingCoinInventoryService();
            var invSnapshot = vis.CreateInventorySnapshot();
            VendingCoinRepository.VendingCoins.Clear();
            VendingCoinRepository.VendingCoins = new List<VendingCoin>()
            {
                new VendingCoin() { Value = .10M, CoinDenomination = CoinDenomination.CENT, Quantity = 100 },
                new VendingCoin() { Value = .20M, CoinDenomination = CoinDenomination.CENT, Quantity = 100 },
                new VendingCoin() { Value = .50M, CoinDenomination = CoinDenomination.CENT, Quantity = 100 },
            };

            var result = vis.PickCoinsForChange(1.30M);
            var changeAmount = result.Sum(s => s.Amount);

            Assert.Equal(1.30M, changeAmount);
            Assert.Equal(0, result.Count(c => c.CoinDenomination == CoinDenomination.EURO));
            vis.RestoreInventorySnapshot(invSnapshot.ToList());
        }
    }
}
