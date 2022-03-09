using ModelsLib.ContextRepositoryClasses;
using Microsoft.EntityFrameworkCore;

namespace AppContextLib.Data
{
    public interface IAppContext
    {
        DbSet<Account> Accounts { get; set; }

        DbSet<Role> Roles { get; set; }

        DbSet<Transaction> Transactions { get; set; }
        int SaveChangesInDataBase();
    }
}