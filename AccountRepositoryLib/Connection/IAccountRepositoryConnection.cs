using AccountRepositoryLib.Repositories;

namespace AccountRepositoryLib.Connection
{
    public interface IAccountRepositoryConnection
    {
        IAccountRepository Repository { get; }
    }
}