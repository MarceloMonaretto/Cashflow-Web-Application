using AccountInformationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountInformationAPI.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext context)
        {
            _context = context;
        }

        public async Task CreateAccountAsync(AccountModel account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            await _context.Accounts.AddAsync(account);
        }

        public async Task DeleteAccountAsync(int id)
        {
            AccountModel account = await GetAccountByIdAsync(id);

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            _context.Accounts.Remove(account);
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

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public async Task UpdateAccountAsync(AccountModel updatedAccount, int id)
        {
            AccountModel account = await GetAccountByIdAsync(id);

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            account.UserCredentialName = updatedAccount.UserCredentialName;
            account.UserCredentialPassword = updatedAccount.UserCredentialPassword;
            account.Role = updatedAccount.Role;
        }
    }
}
