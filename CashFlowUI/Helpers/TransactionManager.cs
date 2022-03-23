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

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            await _transactionClient.UpdateTransactionAsync(transaction);
        }

        public async Task<(List<double> lastMonthsTotalsPerDay, List<string> lastMonthsDays)> getLastMonthsTotalsPerDayAsync()
        {
            var transactions = await GetAllTransactionsAsync();
            var periodDateTimeBegin = DateTime.Now.AddDays(-30);
            var thisDay = DateOnly.FromDateTime(periodDateTimeBegin);
            List<string> lastMonthsDates = new();
            transactions = transactions.FilterByDateLaterThan(periodDateTimeBegin).OrderBy(t => t.TransactionTime);

            List<double> lastMonthsTotalsPerDay = new();

            var transactionsTimes = transactions
                .Select(t => t.TransactionTime);

            for (var i = 0; i <= 30; i++)
            {
                var thisDaysSum = 0d;
                foreach (var transaction in transactions)
                {
                    var transactionDate = DateOnly.FromDateTime(transaction.TransactionTime);
                    if (transactionDate == thisDay)
                    {
                        thisDaysSum += transaction.Amount;
                    }
                }

                lastMonthsTotalsPerDay.Add(thisDaysSum);
                lastMonthsDates.Add($"{thisDay.Month}/{thisDay.Day}");
                thisDay = thisDay.AddDays(1);
            }

            return (lastMonthsTotalsPerDay, lastMonthsDates);
        }
    }
}
