using BTL_WEBBH.Models.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBBH.Models
{
    public class Product
    {
        [Key]
        [Column(TypeName = "Varchar"), StringLength(50)]
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(250, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProName { get; set; }
        
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal OldPrice { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Image { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please Choose a category.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Relationship with Brand
        [Required,Range(1,int.MaxValue,ErrorMessage = "Please Choose a brand.")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
