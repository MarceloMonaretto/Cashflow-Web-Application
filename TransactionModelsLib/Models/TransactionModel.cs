using System.ComponentModel.DataAnnotations;

namespace TransactionModelsLib.ContextModelClasses
{
    public class TransactionModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PaymentType { get; set; }

        [Required]
        public DateTime TransactionTime { get; set; }

    }
}
