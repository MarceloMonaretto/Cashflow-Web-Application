using CashFlowUI.Extensions;
using CashFlowUI.Helpers;
using CashFlowUI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Controllers
{
    [Authorize(Roles = "Manager, Employee")]
    public class TransactionController : Controller
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }
        // GET: AddTransactionController
        public ActionResult AddTransaction(string message = null)
        {
            ViewBag.AddTransactionMessage = message;
            return View();
        }

        [HttpPost]
        [ActionName("CreateTransactionAsync")]
        public async Task<ActionResult> CreateTransactionAsync([FromForm] AddTransactionViewModel addTransactionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("AddTransaction", addTransactionViewModel);
            }

            await _transactionManager.CreateTransaction(addTransactionViewModel.ToTransactionModel());

            return RedirectToAction("AddTransaction", new { message = "Transaction Created!" });
        }

        [ActionName("TransactionListPage")]
        public IActionResult TransactionListPage()
        {
            return View("TransactionList");
        }
        [HttpDelete]
        [ActionName("DeleteTransactionById")]
        public async Task<IActionResult> DeleteTransactionByIdAsync(int id)
        {
            await _transactionManager.DeleteTransactionAsync(id);
            return NoContent();
        }

        [HttpPost]
        [ActionName("GetTransactionList")]
        public async Task<IActionResult> GetTransactionList()
        {
            var start = Convert.ToInt32(Request.Form["start"]);
            var length = Convert.ToInt32(Request.Form["length"]);
            var searchValue = (Request.Form["search[value]"]);
            var columnIndex = (Request.Form["order[0][column]"]);
            var columnName = (Request.Form[$"columns[{columnIndex}][data]"]).ToString();
            var sortDirection = (Request.Form["order[0][dir]"]);

            var transactionList = (await _transactionManager.GetAllTransactionsAsync()).ToList();
            var wasFiltered = false;
            int totalRecords;
            List<Transaction> filteredTransactionList;

            (filteredTransactionList, wasFiltered) = _transactionManager.FilterTransactions(transactionList, Request);
            

            if (!string.IsNullOrEmpty(searchValue))
            {
                filteredTransactionList = filteredTransactionList.
                    Where(x => x.Description.ToLower().Contains(searchValue.ToString().ToLower()) ||
                    x.PaymentType.ToLower().Contains(searchValue.ToString().ToLower())).ToList();
            }
            
            if (length > 0)
            {
                filteredTransactionList = filteredTransactionList.Skip(start).Take(length).ToList();
            }

            if (!string.IsNullOrEmpty(columnName))
                filteredTransactionList = filteredTransactionList.AsQueryable().OrderBy(columnName + " " + sortDirection).ToList();

            if (wasFiltered)
            {
                totalRecords = filteredTransactionList.Count;
            }
            else
            {
                totalRecords = transactionList.Count;
            }

            dynamic response = new
            {
                Data = filteredTransactionList,
                Draw = Request.Form["draw"],
                RecordsFiltered = totalRecords,
                RecordsTotal = filteredTransactionList.Count()
            };

            return Json(response);
        }
    }
}
