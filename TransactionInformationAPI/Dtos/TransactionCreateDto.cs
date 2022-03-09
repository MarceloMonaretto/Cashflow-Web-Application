using System.ComponentModel.DataAnnotations;

namespace TransactionInformationAPI.Dtos
{
    public class TransactionCreateDto
    {
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionTime { get; set; }

    }
}
