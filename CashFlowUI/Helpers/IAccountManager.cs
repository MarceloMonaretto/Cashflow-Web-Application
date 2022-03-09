using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Helpers
{
    public interface IAccountManager
    {
        Task<string> GetUserRoleAsync(string user);
        Task<bool> ValidateLoginInfoAsync(string user, string password);

        Task<Account> GetAccountByUserNameAsync(string userName);
    }
}