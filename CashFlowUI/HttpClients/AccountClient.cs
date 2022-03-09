using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public class AccountClient : IAccountClient
    {

        private readonly HttpClient _httpClient;
        public AccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAccountAsync(Account account)
        {
            HttpResponseMessage result = await _httpClient.PostAsync("account/Create", JsonContent.Create(account));
            result.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync("account/all");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<IEnumerable<Account>>();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.GetAsync($"account/{id}");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<Account>();
        }

        public async Task UpdateAccountAsync(Account account)
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
