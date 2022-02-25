using AccountManagerLib.Models;

namespace CashFlowUI.HttpClients
{
    public interface IAccountClient
    {
        Task CreateAccountAsync(AccountModel account);
        Task DeleteAccountAsync(int id);
        Task<IEnumerable<AccountModel>> GetAllAccountsAsync();
        Task<AccountModel> GetAccountByIdAsync(int id);
        Task UpdateAccountAsync(AccountModel account);
    }
}