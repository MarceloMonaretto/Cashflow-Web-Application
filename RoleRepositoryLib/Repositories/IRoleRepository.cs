using ModelsLib.ContextRepositoryClasses;

namespace RoleRepositoryLib.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<bool> CreateRoleAsync(Role account);
        Task<bool> DeleteRoleByNameAsync(string roleName);
        Task<bool> UpdateRoleAsync(Role updatedRole);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}