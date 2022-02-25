using AccountInformationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountInformationAPI.Data
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AccountModel> Accounts { get; set; }
    }
}
