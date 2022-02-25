using System.ComponentModel.DataAnnotations;

namespace AccountInformationAPI.Dtos
{
    public class AccountCreateDto
    {
        [Required]
        public string UserCredentialName { get; set; }
        [Required]
        public string UserCredentialPassword { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
