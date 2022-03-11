using CashFlowUI.Models;
using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Helpers
{
    public interface ITransactionManager
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        double GetSumOfAllAmounts(IEnumerable<Transaction> transactions);
        Task<IEnumerable<Transaction>> GetTransactionsInInterval(DateTime start, DateTime end);
        Task<SummaryTransactionViewModel> GetSummaryOfTransactionsAsync();
    }
}