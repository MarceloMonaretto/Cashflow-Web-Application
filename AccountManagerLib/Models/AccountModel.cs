using System.ComponentModel.DataAnnotations;

namespace AccountManagerLib.Models
{
    public class AccountModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserCredentialName { get; set; }
        [Required]
        public string UserCredentialPassword { get; set;}
        [Required]
        public string Role { get; set; }
    }
}
