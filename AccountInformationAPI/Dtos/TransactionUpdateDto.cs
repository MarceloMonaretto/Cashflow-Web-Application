using System.ComponentModel.DataAnnotations;

namespace ModelsLib.ContextRepositoryClasses
{
    public class TransactionUpdateDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionTime { get; set; }

    }
}
