using System.ComponentModel;

namespace CashFlowUI.Models
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Payment Type")]
        public string PaymentType { get; set; }

        [DisplayName("Amount")]
        public double Amount { get; set; }

        [DisplayName("Transaction Time")]
        public DateTime TransactionTime { get; set; }
    }
}
