using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public interface IAccountClient
    {
        Task CreateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(int id);
        Task UpdateAccountAsync(Account account);
    }
}