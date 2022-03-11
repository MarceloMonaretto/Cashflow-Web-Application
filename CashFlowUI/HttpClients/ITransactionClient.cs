using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public interface ITransactionClient
    {
        Task CreateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task UpdateTransactionAsync(Transaction transaction);
    }
}