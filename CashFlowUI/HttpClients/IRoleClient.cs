using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.HttpClients
{
    public interface IRoleClient
    {
        Task CreateRoleAsync(Role role);
        Task DeleteRoleAsync(string roleName);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByNameAsync(string roleName);
        Task UpdateRoleAsync(Role role);
    }
}