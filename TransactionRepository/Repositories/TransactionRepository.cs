using ModelsLib.ContextRepositoryClasses;
using Microsoft.EntityFrameworkCore;
using AppContextLib.Data;

namespace TransactionRepositoryLib.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IAppContext _context;

        public TransactionRepository(IAppContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            await _context.Transactions.AddAsync(transaction);
            return SaveChanges();
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            Transaction transaction = await GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            _context.Transactions.Remove(transaction);
            return SaveChanges();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        private bool SaveChanges()
        {
            return _context.SaveChangesInDataBase() >= 0;
        }

        public async Task<bool> UpdateTransactionAsync(Transaction updatedTransaction, int id)
        {
            Transaction transaction = await GetTransactionByIdAsync(id);

            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            transaction.Description = updatedTransaction.Description;
            transaction.PaymentType = updatedTransaction.PaymentType;
            transaction.TransactionTime = updatedTransaction.TransactionTime;

            return SaveChanges();
        }
    }
}
