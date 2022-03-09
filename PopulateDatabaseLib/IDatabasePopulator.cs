
namespace PopulateDatabaseLib
{
    public interface IDatabasePopulator
    {
        Task PopulateAccountsAsync();
        Task PopulateRolesAsync();
        Task PopulateTransitionsAsync();
    }
}