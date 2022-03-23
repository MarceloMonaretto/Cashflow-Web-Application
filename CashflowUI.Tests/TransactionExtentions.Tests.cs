using ModelsLib.ContextRepositoryClasses;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CashFlowUI.Extensions;

namespace CashflowUI.Tests
{
    public class TransactionExtentionsTests
    {
        private readonly IEnumerable<Transaction> _transactionSamples;

        public TransactionExtentionsTests()
        {
            _transactionSamples = GenerateTransactionSamples();
        }

        [Theory]
        [InlineData(4, 8)]
        [InlineData(1, 2)]
        [InlineData(1, 1)]
        [InlineData(2, 23)]
        [InlineData(15, 30)]
        public void GetTransactionsInInterval_IntervalHasTransaction_ShouldReturnTransactions(
            int expectedFirstTransaction, int expectedLastTransaction)
        {
            var expectedTransactions = _transactionSamples
                .Skip(expectedFirstTransaction - 1)
                .Take(expectedLastTransaction - expectedFirstTransaction + 1);

            var startDate = _transactionSamples
                .Skip(expectedFirstTransaction - 1)
                .FirstOrDefault().TransactionTime.ToDateOnly();

            var endDate = _transactionSamples.Skip(expectedFirstTransaction - 1)
                .Take(expectedLastTransaction - expectedFirstTransaction + 1)
                .LastOrDefault().TransactionTime.ToDateOnly();

            var result = _transactionSamples.FilterByDateInInterval(startDate, endDate);

            result.Should().BeEquivalentTo(expectedTransactions);
        }


        [Fact]
        public void GetTransactionsInInterval_IntervalHasNoTransaction_ShouldReturnNull()
        {
            var startDate = new DateOnly(5000, 1, 1);

            var endDate = new DateOnly(5000, 1, 2);

            var result = _transactionSamples.FilterByDateInInterval(startDate, endDate);

            result.Should().BeEmpty();
        }

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
