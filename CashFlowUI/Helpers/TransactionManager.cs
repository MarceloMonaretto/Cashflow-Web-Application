using CashFlowUI.Extensions;
using CashFlowUI.Factories;
using CashFlowUI.HttpClients;
using CashFlowUI.Models;
using Microsoft.Extensions.Primitives;
using ModelsLib.ContextRepositoryClasses;
using System.Linq;
using System.Text.RegularExpressions;

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

        public (List<Transaction>, bool) FilterTransactions(List<Transaction> transactions, HttpRequest request)
        {
            var payLoad = request.Form;
            var transacionsFilteredAux = transactions;
            var searchBuilderFilters = payLoad.Where(d => d.Key.ToLower().Contains("searchbuilder"));
            if (searchBuilderFilters.Any())
            {
                var numberOfUniqueFilters = new List<string>();
                string pattern = @"([(\d+)])";
                foreach (var line in searchBuilderFilters)
                {
                    var filterNumber = Regex.Match(line.Key, pattern);
                    if (!string.IsNullOrEmpty(filterNumber?.Value))
                    {
                        if (!numberOfUniqueFilters.Contains(filterNumber.Value))
                        {
                            numberOfUniqueFilters.Add(filterNumber.Value);
                        }
                    }
                }

                for (var filterNumber = 0; filterNumber < numberOfUniqueFilters.Count; filterNumber++)
                {
                    var builderString = $"searchBuilder[criteria][{filterNumber}]";

                    var filteredTransaction = new List<Transaction>();

                    if (payLoad[builderString + "[type]"] == "date")
                    {
                        filteredTransaction = FilterByDate(transactions, searchBuilderFilters.Where(line => line.Key.Contains(builderString)), filterNumber);
                    }

                    if(filterNumber == 0)
                    {
                        transacionsFilteredAux = filteredTransaction;
                        continue;
                    }
                    else
                    {
                        if(searchBuilderFilters.First(f => f.Key.Contains("logic", StringComparison.OrdinalIgnoreCase)).Value == "AND")
                        {
                            transacionsFilteredAux = transactions.Intersect(filteredTransaction).ToList();
                        }
                        else
                        {
                            transacionsFilteredAux.AddRange(filteredTransaction);
                            transacionsFilteredAux = transacionsFilteredAux.Distinct().ToList();
                        }
                    }
                }

                //transactions = FilterByPaymentType(transactions, payLoad);
            }

            return (transacionsFilteredAux, !transactions.SequenceEqual(transacionsFilteredAux));
        }

        private IEnumerable<Transaction> FilterByPaymentType(IEnumerable<Transaction> transactions, IFormCollection payLoad)
        {
            return null;
        }

        private List<Transaction> FilterByDate(IEnumerable<Transaction> transactions, 
            IEnumerable<KeyValuePair<string,StringValues>> filters, int filterNumber)
        {
            var builderString = $"searchBuilder[criteria][{filterNumber}]";

            var condition = filters.FirstOrDefault(f => f.Key.Contains(builderString + "[condition]")).Value.ToString();
            var date1 = DateTime.Parse(filters.FirstOrDefault(f => f.Key.Contains(builderString + "[value1]")).Value);

            var value2 = filters.FirstOrDefault(f => f.Key.Contains(builderString + "[value2]")).Value;

            if ((condition == "between" || condition == "!between") && DateTime.TryParse(value2, out DateTime date2))
            {
                return FilterByCombinedDateContidions(transactions, date1, date2, condition).ToList();              
            }
            else
            {
                return FilterBySingleDateContidions(transactions, date1, condition).ToList();               
            }
        }

        private IEnumerable<Transaction> FilterBySingleDateContidions(IEnumerable<Transaction> transactions, DateTime date1, string condition)
        {
            switch (condition)
            {
                case "<":
                    return transactions.FilterByDateEarlierThan(date1).ToList();
                case ">":
                    return transactions.FilterByDateLaterThan(date1).ToList();
                case "=":
                    return transactions.FilterByDateEqualTo(date1).ToList();
                case "!=":
                    return transactions.FilterByDateDifferentThan(date1).ToList(); ;
                default:
                    return transactions;
            }
        }

        private IEnumerable<Transaction> FilterByCombinedDateContidions(IEnumerable<Transaction> transactions, DateTime date1, DateTime date2, string condition)
        {
            switch (condition)
            {
                case "between":
                    return transactions.FilterByDateInInterval(date1, date2);
                case "!between":
                    return transactions.FilterByDateNotInInterval(date1, date2);
                default:
                    return transactions;
            }
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

        public async Task CreateTransaction(Transaction transaction)
        {
            await _transactionClient.CreateTransactionAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionClient.DeleteTransactionAsync(id);
        }
    }
}
