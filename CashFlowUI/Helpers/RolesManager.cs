using CashFlowUI.HttpClients;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace CashFlowUI.Helpers
{
    public class RolesManager : IRolesManager
    {
        private readonly IRoleClient _roleClient;
        private readonly HttpContext _httpContext;

        public RolesManager(IHttpContextAccessor httpContextAccessor, IRoleClient roleClient)
        {
            _roleClient = roleClient;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<bool> VerifyRoleMenuPermissionAsync(string roleName, string menuName)
        {
            var role = (await _roleClient.GetAllRolesAsync()).FirstOrDefault(r => r.RoleName == roleName);
            if (role == null)
            {
                return false;
            }

            bool hasPermission = role.MenuAccessPermissions
                .ConvertToListOfStrings()
                .Any(menuPermission => menuPermission == menuName);

            return hasPermission;
        }

        public async Task<bool> VerifyRoleCommandPermissionAsync(string roleName, string commandName)
        {
            var role = (await _roleClient.GetAllRolesAsync()).FirstOrDefault(r => r.RoleName == roleName);
            if (role == null)
            {
                return false;
            }

            bool hasPermission = role.CommandAccessPermissions
                .ConvertToListOfStrings()
                .Any(commandPermission => commandPermission == commandName);

            return hasPermission;
        }

        public string GetUserRoleFromClaims()
        {
            return _httpContext.User.Identities
                .FirstOrDefault()
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role)
                .Value;
        }
    }

    public static class RolesExtensions
    {
        public static IEnumerable<string> ConvertToListOfStrings(this string roles)
        {
            return roles.Split(";").ToList();
        }
    }
}
