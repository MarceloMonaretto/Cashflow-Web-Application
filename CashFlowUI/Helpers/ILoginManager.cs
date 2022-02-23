namespace CashFlowUI.Helpers
{
    public interface ILoginManager
    {
        bool ValidateLoginInfo(string user, string password);

        Task<bool> SignInUserAsync(string user);
        Task<bool> SignOutUserAsync();

        string GetUserRole(string user);
    }
}