using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_WEBBH.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên danh mục")]
        [StringLength(200)]
        public string CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
