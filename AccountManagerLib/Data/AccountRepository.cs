using AccountModelsLib.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountRepositoryLib.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IAccountContext _context;

        public AccountRepository(IAccountContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAccountAsync(AccountModel account)
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
            AccountModel account = await GetAccountByIdAsync(id);

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            _context.Accounts.Remove(account);
            return SaveChanges();
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<AccountModel> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        private bool SaveChanges()
        {
            return _context.SaveChangesInDataBase() >= 0;
        }

        public async Task<bool> UpdateAccountAsync(AccountModel updatedAccount, int id)
        {
            AccountModel account = await GetAccountByIdAsync(id);

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            account.UserCredentialName = updatedAccount.UserCredentialName;
            account.UserCredentialPassword = updatedAccount.UserCredentialPassword;
            account.Role = updatedAccount.Role;

            return SaveChanges();
        }
    }
}
