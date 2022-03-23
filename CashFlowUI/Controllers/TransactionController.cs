using CashFlowUI.Extensions;
using CashFlowUI.Helpers;
using CashFlowUI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using ModelsLib.ContextRepositoryClasses;
using CommandAndMenusLib;

namespace CashFlowUI.Controllers
{
    [Authorize(Roles = "Manager, Employee")]
    public class TransactionController : Controller
    {
        private readonly ITransactionManager _transactionManager;
        private readonly ITransactionsTableManager _transactionsTableManager;
        private readonly IRolesManager _rolesManager;
        private readonly IErrorLogger _errorLogger;

        public TransactionController(ITransactionManager transactionManager,
            ITransactionsTableManager transactionsTableManager, IRolesManager rolesManager, IErrorLogger errorLogger)
        {
            _transactionManager = transactionManager;
            _transactionsTableManager = transactionsTableManager;
            _rolesManager = rolesManager;
            _errorLogger = errorLogger;
        }

        public ActionResult AddTransaction(string message = null)
        {
            ViewBag.AddTransactionMessage = message;
            return View();
        }

        [HttpPost]
        [ActionName("CreateTransaction")]
        public async Task<ActionResult> CreateTransactionAsync([FromBody] AddTransactionViewModel addTransactionViewModel)
        {
            if (!await UserHasAccessToCommandAsync(CommandsNames.CreateTransactionCommand))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View("AddTransaction", addTransactionViewModel);
            }

            try
            {
                await _transactionManager.CreateTransactionAsync(addTransactionViewModel.ToTransactionModel());
            }
            catch (Exception ex)
            {
                _errorLogger.LogErrorMessage(ex.Message);
            }


            return Ok();
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
            if (!await UserHasAccessToCommandAsync(CommandsNames.DeleteTransactionCommand))
            {
                return BadRequest();
            }

            try
            {
                await _transactionManager.DeleteTransactionAsync(id);
            }
            catch (Exception ex)
            {
                _errorLogger.LogErrorMessage(ex.Message);
            }


            return Ok();
        }

        [HttpPut]
        [ActionName("UpdateTransactionById")]
        public async Task<IActionResult> UpdateTransactionByIdAsync([FromBody] Transaction transaction)
        {
            if (!await UserHasAccessToCommandAsync(CommandsNames.EditTransactionCommand))
            {
                return BadRequest();
            }

            try
            {
                await _transactionManager.UpdateTransactionAsync(transaction);
            }
            catch (Exception ex)
            {
                _errorLogger.LogErrorMessage(ex.Message);
            }

            return Ok();
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

            try
            {
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
            catch (Exception ex)
            {
                _errorLogger.LogErrorMessage(ex.Message);
                return NotFound();
            }            
        }

        private async Task<bool> UserHasAccessToCommandAsync(string commandName)
        {
            var userRole = _rolesManager.GetUserRoleFromClaims();
            return await _rolesManager.VerifyRoleCommandPermissionAsync(userRole, commandName);
        }
    }
}
