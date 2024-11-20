using BTL_WEBBH.Models.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBBH.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [Required]
        [Column(TypeName = "varchar"), StringLength(12)]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Column(TypeName = "varchar"), StringLength(36)]
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? AvatarUpload { get; set; }
    }
}
