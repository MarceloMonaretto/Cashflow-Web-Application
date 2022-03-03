using AccountModelsLib.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountRepositoryLib.Data
{
    public interface IAccountContext
    {
        DbSet<AccountModel> Accounts { get; set; }
        int SaveChangesInDataBase();
    }
}