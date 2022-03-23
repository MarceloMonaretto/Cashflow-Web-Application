using ModelsLib.ContextRepositoryClasses;
using CashFlowUI.Helpers;
using CashFlowUI.HttpClients;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using CashFlowUI.Extensions;

namespace CashflowUI.Tests
{
    public class TransactionsTableManagerTests
    {
        private readonly IEnumerable<Transaction> _transactionSamples;

        public TransactionsTableManagerTests()
        {
            _transactionSamples = GenerateTransactionSamples();
        }


        // List<Transaction> FilterTransactions(List<Transaction> allTransactions,
        //    IEnumerable<KeyValuePair<string, StringValues>> searchBuilderFilters);

        // List<Transaction> SearchForText(List<Transaction> filteredTransactionList, StringValues searchValue);

        // List<Transaction> MakePagination(List<Transaction> filteredTransactionList, int start, int length);

        // List<Transaction> SortData(List<Transaction> filteredTransactionList, string columnName, StringValues sortDirection);

        // dynamic CreateUpdatedTableConfiguration(List<Transaction> transactions,
        //    List<Transaction> filteredTransactionList, StringValues formDraw, int totalRecords);


        private IEnumerable<Transaction> GenerateTransactionSamples()
        {
            List<Transaction> transactions = new();
            string paymentType;
            int year = 2000, month = 1, day = 1;
            int hour = 0, minute = 0, second = 0;

            var date = new DateTime(year, month, day, hour, minute, second);

            for (int transactionNumber = 1; transactionNumber <= 24; transactionNumber++)
            {

                if (transactionNumber % 2 == 0)
                    paymentType = "Credit Card";
                else
                    paymentType = "Money";

                var transaction = new Transaction
                {
                    Description = $"Transaction {transactionNumber}",
                    PaymentType = paymentType,
                    Amount = Convert.ToDouble(transactionNumber * 10),
                    TransactionTime = date
                };

                transactions.Add(transaction);
                date = date.AddDays(2).AddHours(1).AddMinutes(2).AddSeconds(3);
            }

            return transactions;
        }
    }
}
