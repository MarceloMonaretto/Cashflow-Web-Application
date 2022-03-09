using RoleRepositoryLib.Repositories;

namespace RoleRepositoryLib.Connection
{
    public class RoleRepositoryConnection : IRoleRepositoryConnection
    {
        public RoleRepositoryConnection(IRoleRepository transactionRepository)
        {
            Repository = transactionRepository;
        }
        public IRoleRepository Repository { get; }
    }
}