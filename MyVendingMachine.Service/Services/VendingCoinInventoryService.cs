using MyVendingMachine.Core.Data;
using MyVendingMachine.Core.Interface;
using MyVendingMachine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace MyVendingMachine.Service.Services
{
    public class VendingCoinInventoryService : IVendingCoinInventoryService
    {
        public IEnumerable<VendingCoin> PickCoinsForChange(decimal changeAmount)
        {
            if (changeAmount == 0)
                return new List<VendingCoin>();
            if (changeAmount < 0)
                throw new ArgumentException("Change Amount is a negative number.");

            IEnumerable<VendingCoin> coinsForChange = new List<VendingCoin>();

            try
            {
                coinsForChange = PickCoins(changeAmount);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return coinsForChange;
        }

        public void AddCoins(IEnumerable<VendingCoin> coins)
        {
            foreach (var coin in coins)
            {
                var invCoin = VendingCoinRepository.VendingCoins
                    .Where(w => w.Value == coin.Value && w.CoinDenomination == coin.CoinDenomination)
                    .FirstOrDefault();

                if (invCoin != null)
                {
                    invCoin.Quantity += coin.Quantity;
                }
                else
                {
                    VendingCoinRepository.VendingCoins.Add(new VendingCoin() { Value = coin.Value, Quantity = coin.Quantity });
                }
            }
        }

        public IEnumerable<VendingCoin> CreateInventorySnapshot()
        {
            var coinInventorySnapshot = VendingCoinRepository.VendingCoins.Select(s => (VendingCoin)s.Clone()).ToList();
            return coinInventorySnapshot;
        }

        public void RestoreInventorySnapshot(IList<VendingCoin> snapshot)
        {
            VendingCoinRepository.VendingCoins.Clear();
            VendingCoinRepository.VendingCoins = snapshot;
        }

        private IEnumerable<VendingCoin> PickCoins(decimal amount)
        {
            List<VendingCoin> coinsForChange = new List<VendingCoin>();

            var coinsInv = VendingCoinRepository.VendingCoins
                .Where(w => w.Quantity > 0)
                .OrderByDescending(o => o.Value).ToList();

            if (amount > 0)
            {
                for (int i = 0; i < coinsInv.Count; i++)
                {
                    var coin = coinsInv[i];
                    if (amount >= coin.Value && coinsInv[i].Quantity > 0)
                    {
                        amount -= coin.Value;
                        coinsInv[i].Quantity -= 1;


                        var coinForChange = coinsForChange.Where(w => w.Value == coin.Value).FirstOrDefault();
                        if (coinForChange != null)
                            coinForChange.Quantity += 1;
                        else
                            coinsForChange.Add(new VendingCoin() { Value = coin.Value, Quantity = 1 });

                        i--;//return to same index - its possible the current value can accomodate change coin
                    }

                    if (amount == 0)
                        break;
                }
            }

            return coinsForChange;
        }
    }
}
