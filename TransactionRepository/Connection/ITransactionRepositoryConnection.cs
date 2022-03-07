using TransactionRepositoryLib.Data;

namespace AccountRepositoryLib.Connection
{
    public interface ITransactionRepositoryConnection
    {
        ITransactionRepository Repository { get; }
    }
}