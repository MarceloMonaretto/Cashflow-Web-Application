using Microsoft.EntityFrameworkCore;

namespace AppContextLib.Data
{
    public static class DbContextOptionsFactory
    {
        private static readonly string _connectionString = "server=localhost;database=CashFlowDb;Trusted_Connection=True;MultipleActiveResultSets=True";
        public static DbContextOptions CreateDbContextOptions()
        {
            var dbContextOptions = new DbContextOptionsBuilder();
            dbContextOptions.UseSqlServer(_connectionString);
            return dbContextOptions.Options;
        }
    }
}
