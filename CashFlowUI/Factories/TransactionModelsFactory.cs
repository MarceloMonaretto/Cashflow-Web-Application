using CashFlowUI.Models;

namespace CashFlowUI.Factories
{
    public static class TransactionModelsFactory
    {
        public static SummaryTransactionViewModel CreateSummaryTransactionViewModel(
            double sumOfTodayTransactions, double sumOfLastMonthTransactions)
        {
            return new SummaryTransactionViewModel()
            {
                TotalToday = sumOfTodayTransactions,
                TotalThisMonth = sumOfLastMonthTransactions
            };
        }
    }
}
