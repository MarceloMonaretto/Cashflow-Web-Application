using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountRepositoryLib.Data
{
    public static class DbContextOptionsFactory
    {
        private static readonly string _connectionString = "server=localhost;database=accountsInfoDb;Trusted_Connection=True;MultipleActiveResultSets=True";
        public static DbContextOptions CreateDbContextOptions()
        {
            var dbContextOptions = new DbContextOptionsBuilder();
            dbContextOptions.UseSqlServer(_connectionString);
            return dbContextOptions.Options;
        }
    }
}
