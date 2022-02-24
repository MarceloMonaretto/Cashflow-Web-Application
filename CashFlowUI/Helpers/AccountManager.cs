namespace CashFlowUI.Helpers
{
    public class AccountManager : IAccountManager
    {
        private readonly List<string[]> Users = new()
        {
            new string[] { "manager", "Boss123", "manager" },
            new string[] { "employee", "Employee123", "staff" },
            new string[] { "testUser", "testPassword", "testRole" }
        };

        public bool ValidateLoginInfo(string user, string password)
        {
            try
            {
                return Users.Single(u => u[0] == user && u[1] == password).Any();
            }
            catch
            {
                return false;
            }
        }

        public string GetUserRole(string user)
        {
            return Users.FirstOrDefault(u => u[0] == user)?[2];
        }
    }
}
