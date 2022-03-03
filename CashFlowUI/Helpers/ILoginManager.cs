namespace CashFlowUI.Helpers
{
    public interface ILoginManager
    {
        Task SignInUserAsync(string user);
        Task SignOutUserAsync();
        Task<bool> CanLoginAsync(string user, string password);
    }
}