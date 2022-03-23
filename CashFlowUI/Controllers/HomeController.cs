using CashFlowUI.Helpers;
using CashFlowUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CashFlowUI.Extensions;
using ModelsLib.ContextRepositoryClasses;
using CashFlowUI.Factories;

namespace CashFlowUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IErrorLogger _errorLogger;

        public HomeController(ITransactionManager transactionManager, IErrorLogger errorLogger)
        {
            _transactionManager = transactionManager;
            _errorLogger = errorLogger;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var transactionsSummaryViewModel = await _transactionManager.GetSummaryOfTransactionsAsync();
                return View("Index", transactionsSummaryViewModel);
            }
            catch (Exception ex)
            {
                _errorLogger.LogErrorMessage(ex.Message);
                var transactionsSummaryViewModel = TransactionModelsFactory.CreateSummaryTransactionViewModel(0, 0, "Could not load data!");
                return View("Index", transactionsSummaryViewModel);
            }   
        }

        [ActionName("GetLastThirtyDaysValues")]
        public async Task<IActionResult> GetLastThirtyDaysValuesAsync()
        {
            try
            {
                (List<double> lastMonthsTotalsPerDay, List<string> lastMonthsDays) = await _transactionManager.getLastMonthsTotalsPerDayAsync();

                return Json(new { amounts = lastMonthsTotalsPerDay, dates = lastMonthsDays });
            }
            catch (Exception)
            {
                return NotFound();
            }
            
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