using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public class PdfClient : IPdfClient
    {

        private readonly HttpClient _httpClient;
        public PdfClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPdf()
        {
            HttpResponseMessage result = await _httpClient.GetAsync("Transaction/getpdf");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsStringAsync();
        }

    }
}
