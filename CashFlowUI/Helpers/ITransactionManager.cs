using CashFlowUI.Models;
using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Helpers
{
    public interface ITransactionManager
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        double GetSumOfAllAmounts(IEnumerable<Transaction> transactions);
        Task<SummaryTransactionViewModel> GetSummaryOfTransactionsAsync();
        Task CreateTransaction(Transaction transaction);
        (List<Transaction>, bool) FilterTransactions(List<Transaction> transactions, HttpRequest request);
        Task DeleteTransactionAsync(int id);
    }
}