using CashFlowUI.Helpers;
using CashFlowUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CashFlowUI.Extensions;
using ModelsLib.ContextRepositoryClasses;

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

        [ActionName("GetLastThirtyDaysValues")]
        public async Task<IActionResult> GetLastThirtyDaysValuesAsync()
        {

            (List<double> lastMonthsTotalsPerDay, List<string> lastMonthsDays) = await _transactionManager.getLastMonthsTotalsPerDayAsync();

            return Json(new { amounts = lastMonthsTotalsPerDay, dates = lastMonthsDays });
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