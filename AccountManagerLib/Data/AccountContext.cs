using AccountModelsLib.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountRepositoryLib.Data
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AccountModel> Accounts { get; set; }
    }
}
