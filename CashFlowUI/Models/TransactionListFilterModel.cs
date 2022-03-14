using System.ComponentModel.DataAnnotations;

namespace CashFlowUI.Models
{
    public class TransactionListFilterModel
    {
        [Display(Name = "Start Date")]
        public DateTime FirstDateForFilter { get; set; }
        [Display(Name = "End Date")]
        public DateTime LastDateForFilter { get; set; }
        [Display(Name = "Payment Type")]
        public string PaymentTypeFilter { get; set; }
    }
}
