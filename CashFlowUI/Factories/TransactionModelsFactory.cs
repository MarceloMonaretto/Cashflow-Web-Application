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
                TotalToday = Math.Round(sumOfTodayTransactions,2,MidpointRounding.ToZero),
                TotalThisMonth = Math.Round(sumOfLastMonthTransactions, 2, MidpointRounding.ToZero)
            };
        }
    }
}
