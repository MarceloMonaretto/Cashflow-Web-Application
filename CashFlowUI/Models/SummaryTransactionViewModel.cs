using System.ComponentModel;

namespace CashFlowUI.Models
{
    public class SummaryTransactionViewModel
    {
        [DisplayName("Today")]
        public double TotalToday { get; set; }

        [DisplayName("Last 30 Days")]
        public double TotalThisMonth { get; set; }

        public string ErrorMessage { get; set; }
    }
}
