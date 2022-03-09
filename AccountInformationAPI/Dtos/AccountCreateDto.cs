using System.ComponentModel.DataAnnotations;

namespace AccountInformationAPI.Dtos
{
    public class AccountCreateDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public string UserRole { get; set; }
    }
}
