using TransactionRepositoryLib.Data;

namespace TransactionRepositoryLib.Connection
{
    public class TransactionRepositoryConnection : ITransactionRepositoryConnection
    {
        public TransactionRepositoryConnection(ITransactionRepository transactionRepository)
        {
            Repository = transactionRepository;
        }
        public ITransactionRepository Repository { get; }
    }
}