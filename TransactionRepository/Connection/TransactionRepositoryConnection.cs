using TransactionRepositoryLib.Data;

namespace AccountRepositoryLib.Connection
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