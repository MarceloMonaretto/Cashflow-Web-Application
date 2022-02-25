using AccountInformationAPI.Models;

namespace AccountInformationAPI.Data
{
    public interface IAccountRepository
    {
        Task CreateAccountAsync(AccountModel account);
        Task DeleteAccountAsync(int id);
        Task<AccountModel> GetAccountByIdAsync(int id);
        Task<IEnumerable<AccountModel>> GetAllAccountsAsync();
        bool SaveChanges();
        Task UpdateAccountAsync(AccountModel updatedAccount, int id);
    }
}