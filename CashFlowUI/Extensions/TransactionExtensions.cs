using CashFlowUI.Models;
using ModelsLib.ContextRepositoryClasses;

namespace CashFlowUI.Extensions
{
    public static class TransactionExtensions
    {
        public static Transaction ToTransactionModel(this AddTransactionViewModel addTransactionModel)
        {
            return new Transaction
            {
                Description = addTransactionModel.Description,
                Amount = addTransactionModel.Amount,
                PaymentType = addTransactionModel.PaymentType,
                TransactionTime = DateTime.Now
            };
        }
    }
}
