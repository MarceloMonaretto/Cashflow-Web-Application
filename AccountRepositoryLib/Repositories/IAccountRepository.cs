using ModelsLib.ContextRepositoryClasses;

namespace AccountRepositoryLib.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> CreateAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(int id);
        Task<Account> GetAccountByIdAsync(int id);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<bool> UpdateAccountAsync(Account updatedAccount, int id);
    }
}