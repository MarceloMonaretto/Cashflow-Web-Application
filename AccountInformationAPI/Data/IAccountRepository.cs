using AccountInformationAPI.Models;

namespace AccountInformationAPI.Data
{
    public interface IAccountRepository
    {
        Task<bool> CreateAccountAsync(AccountModel account);
        Task<bool> DeleteAccountAsync(int id);
        Task<AccountModel> GetAccountByIdAsync(int id);
        Task<IEnumerable<AccountModel>> GetAllAccountsAsync();
        Task UpdateAccountAsync(AccountModel updatedAccount, int id);
    }
}