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
        private readonly ITransactionsTableManager _transactionsTableManager;

        public TransactionController(ITransactionManager transactionManager, ITransactionsTableManager transactionsTableManager)
        {
            _transactionManager = transactionManager;
            _transactionsTableManager = transactionsTableManager;
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

            await _transactionManager.CreateTransactionAsync(addTransactionViewModel.ToTransactionModel());

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

        [HttpPut]
        [ActionName("UpdateTransactionById")]
        public async Task<IActionResult> UpdateTransactionByIdAsync(Transaction transaction)
        {
            await _transactionManager.UpdateTransactionAsync(transaction);
            return NoContent();
        }

        [HttpPost]
        [ActionName("GetTransactionList")]
        public async Task<IActionResult> GetTransactionList()
        {
            var requestFormData = Request.Form;
            var start = Convert.ToInt32(requestFormData["start"]);
            var length = Convert.ToInt32(requestFormData["length"]);
            var searchValue = requestFormData["search[value]"];
            var columnIndex = requestFormData["order[0][column]"];
            var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
            var sortDirection = requestFormData["order[0][dir]"];
            var formDraw = requestFormData["draw"];
            var searchBuilderFilters = requestFormData.Where(d => d.Key.Contains("searchBuilder"));

            var allTransactions = await _transactionManager.GetAllTransactionsAsync();
            List<Transaction> filteredTransactionList = _transactionsTableManager.FilterTransactions(
                allTransactions.ToList(), searchBuilderFilters);

            filteredTransactionList = _transactionsTableManager.SearchForText(filteredTransactionList, searchValue);
            int totalRecords;
            bool wasFiltered = !allTransactions.SequenceEqual(filteredTransactionList);
            if (wasFiltered)
            {
                totalRecords = filteredTransactionList.Count;
            }
            else
            {
                totalRecords = allTransactions.Count();
            }
            filteredTransactionList = _transactionsTableManager.MakePagination(filteredTransactionList, start, length);
            filteredTransactionList = _transactionsTableManager.SortData(filteredTransactionList, columnName, sortDirection);
            dynamic response = _transactionsTableManager.CreateUpdatedTableConfiguration(allTransactions.ToList(),
                filteredTransactionList, formDraw, totalRecords);

            return Json(response);
        }
    }
}
