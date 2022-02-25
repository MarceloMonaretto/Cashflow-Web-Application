using AccountModelsLib.Models;
using CashFlowUI.HttpClients;

namespace CashFlowUI.Helpers
{
    public class AccountManager : IAccountManager
    {
        private readonly IAccountClient _accountClient;

        public AccountManager(IAccountClient accountClient) => _accountClient = accountClient;

        public async Task<bool> ValidateLoginInfoAsync(string userName, string password)
        {
            var accounts = await _accountClient.GetAllAccountsAsync();

            var isValidUser = accounts
                .Where(acc => string.Equals(acc.UserCredentialName, userName, StringComparison.OrdinalIgnoreCase)
                && acc.UserCredentialPassword == password).Any();

            return isValidUser;
        }

        public async Task<AccountModel> GetAccountByUserNameAsync(string userName)
        {
            var accounts = await _accountClient.GetAllAccountsAsync();
            var user = accounts
                .FirstOrDefault(acc => 
                string.Equals(acc.UserCredentialName, userName, StringComparison.OrdinalIgnoreCase));

            return user;
        }

        public async Task<string> GetUserRoleAsync(string userName) => (await GetAccountByUserNameAsync(userName))?.Role;
    }
}
