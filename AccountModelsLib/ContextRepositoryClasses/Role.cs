using System.ComponentModel.DataAnnotations;

namespace ModelsLib.ContextRepositoryClasses
{
    public class Role
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [MaxLength(50)]
        public string RoleName { get; set; }

        [MaxLength(1000)]
        public string MenuAccessPermissions { get; set; }

        [MaxLength(1000)]
        public string CommandAccessPermissions { get; set; }
    }
}
