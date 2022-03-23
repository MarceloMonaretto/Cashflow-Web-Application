using CashFlowUI.Models;
using Microsoft.Extensions.Primitives;
using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Helpers
{
    public interface ITransactionManager
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        double GetSumOfAllAmounts(IEnumerable<Transaction> transactions);
        Task<SummaryTransactionViewModel> GetSummaryOfTransactionsAsync();
        Task CreateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id);
        Task UpdateTransactionAsync(Transaction transaction);
        Task<(List<double> lastMonthsTotalsPerDay, List<string> lastMonthsDays)> getLastMonthsTotalsPerDayAsync();
    }
}