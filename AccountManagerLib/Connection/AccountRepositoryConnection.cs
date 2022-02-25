using repository = AccountRepositoryLib.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountRepositoryLib.Connection
{
    public class AccountRepositoryConnection : IAccountRepositoryConnection
    {
        private readonly repository.AccountContext _context;
        private const string connectionString = "server=localhost;database=accountsInfoDb;Trusted_Connection=True;MultipleActiveResultSets=True";

        public AccountRepositoryConnection()
        {
            var dbContextOptions = new DbContextOptionsBuilder();
            dbContextOptions.UseSqlServer(connectionString);

            _context = new repository.AccountContext(dbContextOptions.Options);
        }
        public repository.IAccountRepository Repository => new repository.AccountRepository(_context);
    }
}