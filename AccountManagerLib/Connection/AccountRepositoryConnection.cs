using AccountRepositoryLib.Data;

namespace AccountRepositoryLib.Connection
{
    public class AccountRepositoryConnection : IAccountRepositoryConnection
    {
        public AccountRepositoryConnection(IAccountRepository accountRepository)
        {
            Repository = accountRepository;
        }
        public IAccountRepository Repository { get; }
    }
}