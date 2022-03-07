
namespace CashFlowUI.Helpers
{
    public interface IRolesManager
    {
        string GetUserRoleFromClaims();
        Task<bool> VerifyRoleCommandPermissionAsync(string roleName, string commandName);
        Task<bool> VerifyRoleMenuPermissionAsync(string roleName, string menuName);
    }
}