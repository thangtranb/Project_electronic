using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BTL_WEBBH.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [Column(TypeName = "varchar"), StringLength(36)]
        public string AccountId { get; set; } = null!;
        [StringLength(100)]
        public string Fullname { get; set; } = null!;
        [StringLength(100)]
        public string Username { get; set; } = null!;
        [StringLength(256), Column(TypeName = "varchar")]
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; } = true;
        public string Picture { get; set; } = string.Empty;
        
    }
}
