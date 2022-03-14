using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CashFlowUI.Models
{
    public class AddTransactionViewModel
    {
        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Payment Type")]
        public string PaymentType { get; set; }

        [Required]
        [DisplayName("Amount")]
        public double Amount { get; set; }
    }
}
