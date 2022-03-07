using ModelsLib.ContextRepositoryClasses;

namespace TransactionRepositoryLib.Data
{
    public interface ITransactionRepository
    {
        Task<bool> CreateTransactionAsync(Transaction account);
        Task<bool> DeleteTransactionAsync(int id);
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<bool> UpdateTransactionAsync(Transaction updatedAccount, int id);
    }
}