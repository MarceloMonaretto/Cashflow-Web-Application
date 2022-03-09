using ModelsLib.ContextRepositoryClasses;
using Microsoft.EntityFrameworkCore;

namespace AppContextLib.Data
{
    public class AppContext : DbContext , IAppContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public int SaveChangesInDataBase() => base.SaveChanges();
    }
}
