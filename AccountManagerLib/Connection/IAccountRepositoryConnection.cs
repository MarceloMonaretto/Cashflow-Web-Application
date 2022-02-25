using AccountRepositoryLib.Data;

namespace AccountRepositoryLib.Connection
{
    public interface IAccountRepositoryConnection
    {
        IAccountRepository Repository { get; }
    }
}