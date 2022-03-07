using ModelsLib.ContextRepositoryClasses;
using Microsoft.EntityFrameworkCore;
using AppContextLib.Data;

namespace AccountRepositoryLib.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IAppContext _context;

        public AccountRepository(IAppContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            await _context.Accounts.AddAsync(account);
            return SaveChanges();
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            Account account = await GetAccountByIdAsync(id);

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            _context.Accounts.Remove(account);
            return SaveChanges();
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        private bool SaveChanges()
        {
            return _context.SaveChangesInDataBase() >= 0;
        }

        public async Task<bool> UpdateAccountAsync(Account updatedAccount, int id)
        {
            Account account = await GetAccountByIdAsync(id);

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            account.UserName = updatedAccount.UserName;
            account.UserPassword = updatedAccount.UserPassword;
            account.UserRole = updatedAccount.UserRole;

            return SaveChanges();
        }
    }
}
