using CashFlowUI.Extensions;
using CashFlowUI.Factories;
using CashFlowUI.HttpClients;
using CashFlowUI.Models;
using Microsoft.Extensions.Primitives;
using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Helpers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly ITransactionClient _transactionClient;

        public TransactionManager(ITransactionClient transactionClient) => _transactionClient = transactionClient;

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync() =>
            await _transactionClient.GetAllTransactionsAsync();

        public IEnumerable<Transaction> FilterTransactionsByInterval(IEnumerable<Transaction> transactions,
            DateTime start, DateTime end)
        {
            var validTransactions = transactions?
                .Where(t => DateTime.Compare(t.TransactionTime, start) >= 0 && DateTime.Compare(t.TransactionTime, end) <= 0);

            return validTransactions;
        }

        public double GetSumOfAllAmounts(IEnumerable<Transaction> transactions) => transactions.Sum(t => t.Amount);

        public async Task<SummaryTransactionViewModel> GetSummaryOfTransactionsAsync()
        {
            var now = DateTime.Now;
            var aMonthAgo = now.AddDays(-30);
            var today = new DateTime(now.Year, now.Month, now.Day);

            var transactions = await _transactionClient.GetAllTransactionsAsync();
            var transactionsOfToday = transactions.FilterByDateInInterval(today, now);
            var sumOfTodaysTransactions = GetSumOfAllAmounts(transactionsOfToday);
            var transactionsOfLastMonth = transactions.FilterByDateInInterval(aMonthAgo, now);
            var sumOfLastMonthsTransactions = GetSumOfAllAmounts(transactionsOfLastMonth);

            return TransactionModelsFactory.CreateSummaryTransactionViewModel(
                sumOfTodaysTransactions, sumOfLastMonthsTransactions);
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            await _transactionClient.CreateTransactionAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionClient.DeleteTransactionAsync(id);
        }
    }
}
