using CashFlowUI.Helpers;
using CashFlowUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CashFlowUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITransactionManager _transactionManager;

        public HomeController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var transactionsSummaryViewModel = await _transactionManager.GetSummaryOfTransactionsAsync();
            return View("Index", transactionsSummaryViewModel);
        }

        public IActionResult Privacy()
        {
            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}