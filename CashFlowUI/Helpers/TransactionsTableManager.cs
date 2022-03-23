using CashFlowUI.Extensions;
using Microsoft.Extensions.Primitives;
using ModelsLib.ContextRepositoryClasses;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core;

namespace CashFlowUI.Helpers
{
    public class TransactionsTableManager : ITransactionsTableManager
    {
        public List<Transaction> FilterTransactions(List<Transaction> allTransactions, 
            IEnumerable<KeyValuePair<string, StringValues>> searchBuilderFilters)
        {
            var transacionsFilteredResult = allTransactions;
            if (!searchBuilderFilters.Any())
                return allTransactions;

            var numberOfUniqueFilters = FindNumberOfUniqueFilters(searchBuilderFilters).Count;
            for (var filterNumber = 0; filterNumber < numberOfUniqueFilters; filterNumber++)
            {
                var builderString = $"searchBuilder[criteria][{filterNumber}]";
                var filteredTransactionsForThisStep = new List<Transaction>();
                if (searchBuilderFilters.FirstOrDefault(f => f.Key == builderString + "[type]").Value == "date")
                    filteredTransactionsForThisStep = FilterByDate(allTransactions, searchBuilderFilters
                        .Where(line => line.Key.Contains(builderString)), filterNumber);
                
                if (searchBuilderFilters.FirstOrDefault(f => f.Key == builderString + "[origData]").Value == "paymentType")
                    filteredTransactionsForThisStep = FilterByPaymentType(allTransactions, searchBuilderFilters
                        .Where(line => line.Key.Contains(builderString)), filterNumber);

                transacionsFilteredResult = ProcessFilterLogic(filterNumber, searchBuilderFilters,
                    transacionsFilteredResult, filteredTransactionsForThisStep);                
            }

            return transacionsFilteredResult;
        }

        private static List<Transaction> ProcessFilterLogic(int filterNumber, 
            IEnumerable<KeyValuePair<string, StringValues>> searchBuilderFilters,
            List<Transaction> transacionsFilteredResult, List<Transaction> filteredTransactions)
        {
            var firstFilter = 0;

            if (filterNumber == firstFilter)
            {
                transacionsFilteredResult = filteredTransactions;
            }
            else
            {
                var filterLogic = searchBuilderFilters.First(f => f.Key.Contains("logic", StringComparison.OrdinalIgnoreCase)).Value;
                if (filterLogic == "AND")
                {
                    transacionsFilteredResult = transacionsFilteredResult.Intersect(filteredTransactions).ToList();
                }
                else
                {
                    transacionsFilteredResult.AddRange(filteredTransactions);
                    transacionsFilteredResult = transacionsFilteredResult.Distinct().ToList();
                }
            }

            return transacionsFilteredResult;
        }

        private static List<string> FindNumberOfUniqueFilters(IEnumerable<KeyValuePair<string, StringValues>> searchBuilderFilters)
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

            return numberOfUniqueFilters;
        }

        private static List<Transaction> FilterByPaymentType(IEnumerable<Transaction> transactions,
            IEnumerable<KeyValuePair<string, StringValues>> filters, int filterNumber)
        {
            var builderString = $"searchBuilder[criteria][{filterNumber}]";
            var paymentTypeToCompare = filters.FirstOrDefault(f => f.Key.Contains(builderString + "[value1]")).Value;
            var condition = filters.FirstOrDefault(f => f.Key.Contains(builderString + "[condition]")).Value.ToString();
            return condition switch
            {
                "!=" => transactions.FilterByPaymentTypeDifferentThan(paymentTypeToCompare).ToList(),
                "=" => transactions.FilterByPaymentTypeEqualTo(paymentTypeToCompare).ToList(),
                _ => transactions.ToList()
            };
        }

        private static List<Transaction> FilterByDate(IEnumerable<Transaction> transactions,
            IEnumerable<KeyValuePair<string, StringValues>> filters, int filterNumber)
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

        private static IEnumerable<Transaction> FilterBySingleDateContidions(IEnumerable<Transaction> transactions, DateTime date1, string condition)
        {
            return condition switch
            {
                "<" => transactions.FilterByDateEarlierThan(date1).ToList(),
                ">" => transactions.FilterByDateLaterThan(date1).ToList(),
                "=" => transactions.FilterByDateEqualTo(date1).ToList(),
                "!=" => transactions.FilterByDateDifferentThan(date1).ToList(),
                _ => transactions
            };
        }

        private static IEnumerable<Transaction> FilterByCombinedDateContidions(IEnumerable<Transaction> transactions, DateTime date1, DateTime date2, string condition)
        {
            return condition switch
            {
                "between" => transactions.FilterByDateInInterval(date1, date2),
                "!between" => transactions.FilterByDateNotInInterval(date1, date2),
                _ => transactions,
            };
        }

        public List<Transaction> SearchForText(List<Transaction> transactionList, StringValues searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                transactionList = transactionList.
                    Where(x => x.Description.ToLower().Contains(searchValue.ToString().ToLower()) ||
                    x.PaymentType.ToLower().Contains(searchValue.ToString().ToLower())).ToList();
            }

            return transactionList;
        }

        public List<Transaction> MakePagination(List<Transaction> transactionList, int start, int length)
        {
            if (length > 0)
            {
                transactionList = transactionList.Skip(start).Take(length).ToList();
            }
            return transactionList;
        }

        public List<Transaction> SortData(List<Transaction> transactionList, string columnName, StringValues sortDirection)
        {
            if (!string.IsNullOrEmpty(columnName))
                transactionList = transactionList.AsQueryable().OrderBy(columnName + " " + sortDirection).ToList();
            return transactionList;
        }

        public dynamic CreateUpdatedTableConfiguration(List<Transaction> allTransactions,
            List<Transaction> filteredTransactions, StringValues formDraw, int totalRecords)
        {
            dynamic response = new
            {
                Data = filteredTransactions,
                Draw = formDraw,
                RecordsFiltered = totalRecords,
                RecordsTotal = filteredTransactions.Count
            };

            return response;
        }
    }
}
