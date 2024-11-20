using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBBH.Models
{
    [Table("Brand")]
    public class Brand
    {
        
        [Key]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(150, ErrorMessage = "Brand name cannot exceed 150 characters.")]
        public string BrandName { get; set; }

        // One Brand can have many Products
        public ICollection<Product> Products { get; set; }
    }
}
