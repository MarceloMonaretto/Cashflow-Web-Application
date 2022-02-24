namespace CashFlowUI.Helpers
{
    public interface IAccountManager
    {
        string GetUserRole(string user);
        bool ValidateLoginInfo(string user, string password);
    }
}