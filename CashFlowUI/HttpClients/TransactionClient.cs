using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public class TransactionClient : ITransactionClient
    {

        private readonly HttpClient _httpClient;
        public TransactionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            HttpResponseMessage result = await _httpClient.PostAsync("transaction/Create", JsonContent.Create(transaction));
            result.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync("transaction/all");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<IEnumerable<Transaction>>();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.GetAsync($"transaction/{id}");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<Transaction>();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            var result = await _httpClient.PutAsync($"transaction/Update/{transaction.Id}", JsonContent.Create(transaction));
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"transaction/Delete/{id}");
            result.EnsureSuccessStatusCode();
        }
    }
}
