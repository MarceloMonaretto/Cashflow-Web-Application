using System.ComponentModel.DataAnnotations;

namespace ModelsLib.ContextRepositoryClasses
{
    public class Transaction
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
