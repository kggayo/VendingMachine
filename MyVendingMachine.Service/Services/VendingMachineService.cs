using MyVendingMachine.Core.Data;
using MyVendingMachine.Core.Interface;
using MyVendingMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MyVendingMachine.Service.Services
{
    public class VendingMachineService : IVendingMachineService
    {
        private readonly IVendingCoinInventoryService vendingCoinInventoryService;

        public VendingMachineService(IVendingCoinInventoryService vendingCoinInventoryService)
        {
            this.vendingCoinInventoryService = vendingCoinInventoryService;
        }

        public IEnumerable<VendingItem> GetVendingItems()
        {
            return VendingItemRepository.VendingItems;
        }

        public TransactionResult WithdrawVendingItem(VendingItem item, IEnumerable<VendingCoin> insertedCoins)
        {
            TransactionResult result = new TransactionResult();

            IEnumerable<VendingCoin> coinInventorySnapshot = null;
            try
            {
                if (item == null)
                    throw new ArgumentNullException("Item is null");

                if (insertedCoins == null || insertedCoins.Count() == 0)
                    throw new ArgumentNullException("Inserted coins is empty");

                decimal insertedCoinAmount = insertedCoins.Sum(s => s.Amount);
                if (insertedCoinAmount < item.Price)
                    throw new ArgumentException("Insuffecient amount. Please insert more coins.");

                var itemInv = VendingItemRepository.VendingItems.Where(w => w.Name == item.Name).FirstOrDefault();
                if (itemInv.Quantity <= 0)
                    throw new ArgumentException("No stocks available. Please choose another item.");

                decimal changeAmount = insertedCoinAmount - item.Price;

                coinInventorySnapshot = vendingCoinInventoryService.CreateInventorySnapshot();

                vendingCoinInventoryService.AddCoins(insertedCoins);
                var coinsForChange = vendingCoinInventoryService.PickCoinsForChange(changeAmount);

                decimal coinsForChangeAmount = coinsForChange.Sum(s => s.Amount);
                if (coinsForChangeAmount == changeAmount)
                {
                    itemInv.Quantity -= 1;

                    result.IsSuccess = true;
                    result.Message = "Thank You.";
                    result.ReturnedAmount = coinsForChangeAmount;
                    result.ReturnedCoins = coinsForChange;
                }
                else
                {
                    throw new Exception("Not enough coins for change. Please give an exact amount.");
                }
            }
            catch (Exception e)
            {
                if (coinInventorySnapshot != null)
                {
                    vendingCoinInventoryService.RestoreInventorySnapshot(coinInventorySnapshot.ToList());
                }

                result.IsSuccess = false;
                result.Message = e.Message;
                result.ReturnedAmount = insertedCoins.Sum(s => s.Amount);
                result.ReturnedCoins = insertedCoins;
            }

            return result;
        }
    }
}
