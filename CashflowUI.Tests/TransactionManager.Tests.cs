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

namespace CashflowUI.Tests
{
    public class TransactionManagerTests
    {
        private readonly ITransactionManager _transactionManager;
        private readonly IEnumerable<Transaction> _transactionSamples;

        public TransactionManagerTests()
        {
            _transactionSamples = GenerateTransactionSamples();
            _transactionManager = new TransactionManager(MockTransactionClient().Object);
        }

        [Theory]
        [InlineData(4, 8)]
        [InlineData(1, 2)]
        [InlineData(1, 1)]
        [InlineData(2, 23)]
        [InlineData(15, 30)]
        public async Task GetTransactionsInInterval_IntervalHasTransaction_ShouldReturnTransactions(
            int expectedFirstTransaction, int expectedLastTransaction)
        {
            var expectedTransactions = _transactionSamples
                .Skip(expectedFirstTransaction - 1)
                .Take(expectedLastTransaction - expectedFirstTransaction + 1);

            var startDate = _transactionSamples
                .Skip(expectedFirstTransaction - 1)
                .FirstOrDefault().TransactionTime;

            var endDate = _transactionSamples.Skip(expectedFirstTransaction - 1)
                .Take(expectedLastTransaction - expectedFirstTransaction + 1)
                .LastOrDefault().TransactionTime;

            var result = await _transactionManager.GetTransactionsInInterval(startDate, endDate);

            result.Should().BeEquivalentTo(expectedTransactions);
        }


        [Fact]
        public async Task GetTransactionsInInterval_IntervalHasNoTransaction_ShouldReturnNull()
        {
            var startDate = new DateTime(5000, 1, 1);

            var endDate = new DateTime(5000, 1, 2);

            var result = await _transactionManager.GetTransactionsInInterval(startDate, endDate);

            result.Should().BeEmpty();
        }

        private Mock<ITransactionClient> MockTransactionClient()
        {
            var transactionClientMock = new Mock<ITransactionClient>();
            transactionClientMock.Setup(x => x.CreateTransactionAsync(It.IsAny<Transaction>())).Returns(Task.FromResult("Created the transaction!"));
            transactionClientMock.Setup(x => x.DeleteTransactionAsync(It.IsAny<int>())).Returns(Task.FromResult("Deleted the transaction!"));
            transactionClientMock.Setup(x => x.UpdateTransactionAsync(It.IsAny<Transaction>())).Returns(Task.FromResult("Updated the transaction!"));
            transactionClientMock.Setup(x => x.GetTransactionByIdAsync(It.IsAny<int>())).Returns((int id) => Task.FromResult(_transactionSamples.FirstOrDefault(t => t.Id == id)));
            transactionClientMock.Setup(x => x.GetAllTransactionsAsync()).Returns(Task.FromResult(_transactionSamples));

            return transactionClientMock;
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
