using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AppContextLib.Data
{
    public class DbContextOptionsFactory : IDesignTimeDbContextFactory<AppContext>
    {
        public AppContext CreateDbContext(string[] args)
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppContext>();
            dbContextOptions.UseSqlServer(args[0]);
            return new AppContext(dbContextOptions.Options);
        }
    }
}
