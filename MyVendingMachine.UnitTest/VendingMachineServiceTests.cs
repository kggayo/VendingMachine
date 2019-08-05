using MyVendingMachine.Core.Data;
using MyVendingMachine.Service.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using MyVendingMachine.Core.Models;

namespace MyVendingMachine.UnitTest
{
    [Collection("Sequential")]
    public class VendingMachineServiceTests
    {
        [Fact]
        public void WithdrawVendingItem_WithdrawItemNoChange_ReturnsThankYouNoChange()
        {
            List<VendingCoin> insertedCoins = new List<VendingCoin>() {
                new VendingCoin(){ Value = 1, Quantity = 1, CoinDenomination = CoinDenomination.EURO },
                new VendingCoin(){ Value = 20, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 50, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 10, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
            };
            VendingMachineService vms = new VendingMachineService(new VendingCoinInventoryService());

            var result = vms.WithdrawVendingItem(new VendingItem() { Name = "Tea", Price = 1.80M, Quantity = 10 },
                insertedCoins);

            Assert.Equal(0, result.ReturnedAmount);
            Assert.Contains("Thank You", result.Message);
        }

        [Fact]
        public void WithdrawVendingItem_WithdrawItemWithChange_ReturnsChange()
        {
            List<VendingCoin> insertedCoins = new List<VendingCoin>() {
                new VendingCoin(){ Value = 1, Quantity = 1, CoinDenomination = CoinDenomination.EURO },
                new VendingCoin(){ Value = 20, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 50, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 10, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 10, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
            };
            VendingMachineService vms = new VendingMachineService(new VendingCoinInventoryService());

            var result = vms.WithdrawVendingItem(new VendingItem() { Name = "Tea", Price = 1.80M, Quantity = 10 },
                insertedCoins);

            Assert.Equal(.10M, result.ReturnedAmount);
            Assert.Contains("Thank You", result.Message);
        }

        [Fact]
        public void WithdrawVendingItem_WithdrawItemNoChange_ReturnsInsertedCoins()
        {
            List<VendingCoin> insertedCoins = new List<VendingCoin>() {
                new VendingCoin(){ Value = 1, Quantity = 1, CoinDenomination = CoinDenomination.EURO },
                new VendingCoin(){ Value = 50, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 50, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
            };
            VendingCoinInventoryService vis = new VendingCoinInventoryService();
            var invSnapshot = vis.CreateInventorySnapshot();
            VendingCoinRepository.VendingCoins.Clear();
            VendingCoinRepository.VendingCoins = new List<VendingCoin>()
            {
                new VendingCoin() { Value = 10, CoinDenomination = CoinDenomination.CENT, Quantity = 1 },
            };
            VendingMachineService vms = new VendingMachineService(new VendingCoinInventoryService());

            var result = vms.WithdrawVendingItem(new VendingItem() { Name = "Tea", Price = 1.80M, Quantity = 10 },
                insertedCoins);

            Assert.Equal(2, result.ReturnedAmount);
            Assert.Contains("Not enough coins", result.Message);
            vis.RestoreInventorySnapshot(invSnapshot.ToList());
        }

        [Fact]
        public void WithdrawVendingItem_WithdrawItemReturnChangeFromInsertedCoins_ReturnsFewerCoinsChange()
        {
            List<VendingCoin> insertedCoins = new List<VendingCoin>() {
                new VendingCoin(){ Value = 1, Quantity = 1, CoinDenomination = CoinDenomination.EURO },
                new VendingCoin(){ Value = 20, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 50, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 10, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 20, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
            };
            VendingCoinInventoryService vis = new VendingCoinInventoryService();
            var invSnapshot = vis.CreateInventorySnapshot();
            VendingCoinRepository.VendingCoins.ToList().Clear();
            VendingCoinRepository.VendingCoins = new List<VendingCoin>()
            {
                new VendingCoin() { Value = 10, CoinDenomination = CoinDenomination.CENT, Quantity = 1 },
            };
            VendingMachineService vms = new VendingMachineService(new VendingCoinInventoryService());

            var result = vms.WithdrawVendingItem(new VendingItem() { Name = "Tea", Price = 1.80M, Quantity = 10 },
                insertedCoins);

            Assert.Equal(.20M, result.ReturnedAmount);
            Assert.Equal(.20M, result.ReturnedCoins.Where(w => w.Value == 20).Select(s => s.Amount).FirstOrDefault());
            Assert.Contains("Thank You", result.Message);
            vis.RestoreInventorySnapshot(invSnapshot.ToList());
        }

        [Fact]
        public void WithdrawVendingItem_WithdrawItemNoStocks_ReturnsInsertedCoins()
        {
            List<VendingCoin> insertedCoins = new List<VendingCoin>() {
                new VendingCoin(){ Value = 1, Quantity = 1, CoinDenomination = CoinDenomination.EURO },
                new VendingCoin(){ Value = 50, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
                new VendingCoin(){ Value = 50, Quantity = 1, CoinDenomination = CoinDenomination.CENT },
            };

            VendingMachineService vms = new VendingMachineService(new VendingCoinInventoryService());
            var tea = VendingItemRepository.VendingItems.Where(w => w.Name == "Tea").FirstOrDefault();
            tea.Quantity = 0;

            var result = vms.WithdrawVendingItem(new VendingItem() { Name = "Tea", Price = 1.80M, Quantity = 1 },
                insertedCoins);

            tea.Quantity = 10;
            Assert.Equal(2, result.ReturnedAmount);
            Assert.Contains("No stocks available", result.Message);
        }
    }
}
