using CashFlowUI.Factories;
using CashFlowUI.HttpClients;
using CashFlowUI.Models;
using ModelsLib.ContextRepositoryClasses;
using System.Linq;

namespace CashFlowUI.Helpers
{
    public class TransactionManager : ITransactionManager
    {
        private readonly ITransactionClient _transactionClient;

        public TransactionManager(ITransactionClient transactionClient) => _transactionClient = transactionClient;

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync() =>
            await _transactionClient.GetAllTransactionsAsync();

        public async Task<IEnumerable<Transaction>> GetTransactionsInInterval(DateTime start, DateTime end)
        {
            var transactions = await GetAllTransactionsAsync();

            var validTransactions = transactions
                .Where(t => DateTime.Compare(t.TransactionTime, start) >= 0 && DateTime.Compare(t.TransactionTime, end) <= 0);

            return validTransactions;
        }

        public double GetSumOfAllAmounts(IEnumerable<Transaction> transactions) => transactions.Sum(t => t.Amount);

        public async Task<SummaryTransactionViewModel> GetSummaryOfTransactionsAsync()
        {
            var now = DateTime.Now;
            var aMonthAgo = now.AddDays(-30);
            var today = new DateTime(now.Year, now.Month, now.Day);

            var transactionsOfToday = await GetTransactionsInInterval(today, now);
            var sumOfTodayTransactions = GetSumOfAllAmounts(transactionsOfToday);
            var transactionsOfLastMonth = await GetTransactionsInInterval(aMonthAgo, now);
            var sumOfLastMonthTransactions = GetSumOfAllAmounts(transactionsOfLastMonth);

            return TransactionModelsFactory.CreateSummaryTransactionViewModel(
                sumOfTodayTransactions, sumOfLastMonthTransactions);
        }
    }
}
