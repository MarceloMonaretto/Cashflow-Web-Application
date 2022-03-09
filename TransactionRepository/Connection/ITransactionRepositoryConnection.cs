using TransactionRepositoryLib.Data;

namespace TransactionRepositoryLib.Connection
{
    public interface ITransactionRepositoryConnection
    {
        ITransactionRepository Repository { get; }
    }
}