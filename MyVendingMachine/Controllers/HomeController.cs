using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyVendingMachine.Core.Interface;
using MyVendingMachine.Core.Models;
using MyVendingMachine.Models;

namespace MyVendingMachine.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVendingMachineService vendingMachineService;
        public HomeController(IVendingMachineService vendingMachineService)
        {
            this.vendingMachineService = vendingMachineService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetVendingItems()
        {
            var products = vendingMachineService.GetVendingItems();

            return Json(products);
        }

        [HttpPost]
        public IActionResult WithdrawVendingItem(VendingItem item, IEnumerable<VendingCoin> coins)
        {
            var result = vendingMachineService.WithdrawVendingItem(item, coins);

            return Json(result);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
