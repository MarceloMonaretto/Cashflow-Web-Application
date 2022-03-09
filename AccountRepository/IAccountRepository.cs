using AccountRepositoryLib.Models;

namespace AccountRepository.Data
{
    public interface IAccountRepository
    {
        Task<bool> CreateAccountAsync(AccountModel account);
        Task<bool> DeleteAccountAsync(int id);
        Task<AccountModel> GetAccountByIdAsync(int id);
        Task<IEnumerable<AccountModel>> GetAllAccountsAsync();
        Task<bool> UpdateAccountAsync(AccountModel updatedAccount, int id);
    }
}