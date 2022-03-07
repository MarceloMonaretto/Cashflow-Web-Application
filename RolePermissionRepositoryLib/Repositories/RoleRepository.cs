using ModelsLib.ContextRepositoryClasses;
using Microsoft.EntityFrameworkCore;
using AppContextLib.Data;

namespace RoleRepositoryLib.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IAppContext _context;

        public RoleRepository(IAppContext context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            Role role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return role;
        }

        public async Task<bool> CreateRoleAsync(Role role)
        {
            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await _context.Roles.AddAsync(role);

            return SaveChanges();
        }

        public async Task<bool> DeleteRoleByNameAsync(string roleName)
        {
            Role role = await GetRoleByNameAsync(roleName);

            _context.Roles.Remove(role);
            return SaveChanges();
        }

        public async Task<bool> UpdateRoleAsync(Role updatedRole)
        {
            Role role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == updatedRole.RoleName);

            if (role is null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            role.MenuAccessPermissions = updatedRole.MenuAccessPermissions;
            role.CommandAccessPermissions = updatedRole.CommandAccessPermissions;

            return SaveChanges();
        }

        private bool SaveChanges()
        {
            return _context.SaveChangesInDataBase() >= 0;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
