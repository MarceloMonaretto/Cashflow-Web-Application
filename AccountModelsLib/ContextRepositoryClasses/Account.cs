using System.ComponentModel.DataAnnotations;

namespace ModelsLib.ContextRepositoryClasses
{
    public class Account
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPassword { get; set;}
        [Required]
        public string UserRole { get; set; }
    }
}
