using AccountManagerLib.Models;

namespace CashFlowUI.HttpClients
{
    public class AccountClient : IAccountClient
    {

        private readonly HttpClient _httpClient;
        public AccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAccountAsync(AccountModel account)
        {
            HttpResponseMessage result = await _httpClient.PostAsync("account/Create", JsonContent.Create(account));
            result.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<AccountModel>> GetAllAccountsAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync("account/all");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<IEnumerable<AccountModel>>();
        }

        public async Task<AccountModel> GetAccountByIdAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.GetAsync($"account/{id}");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<AccountModel>();
        }

        public async Task UpdateAccountAsync(AccountModel account)
        {
            var result = await _httpClient.PutAsync($"account/Update/{account.Id}", JsonContent.Create(account));
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAccountAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"account/Delete/{id}");
            result.EnsureSuccessStatusCode();
        }
    }
}
