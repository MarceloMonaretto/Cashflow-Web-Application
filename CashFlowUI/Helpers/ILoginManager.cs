namespace CashFlowUI.Helpers
{
    public interface ILoginManager
    {
        Task SignInUserAsync(string user);
        Task SignOutUserAsync();
        bool CanLogin(string user, string password);
    }
}