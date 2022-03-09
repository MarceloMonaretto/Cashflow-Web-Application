using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public class RoleClient : IRoleClient
    {

        private readonly HttpClient _httpClient;
        public RoleClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateRoleAsync(Role role)
        {
            HttpResponseMessage result = await _httpClient.PostAsync("role/Create", JsonContent.Create(role));
            result.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync("role/All");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<IEnumerable<Role>>();
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            HttpResponseMessage result = await _httpClient.GetAsync($"role/{roleName}");
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<Role>();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            var result = await _httpClient.PutAsync($"role/Update/{role.RoleName}", JsonContent.Create(role));
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"role/Delete/{roleName}");
            result.EnsureSuccessStatusCode();
        }
    }
}
