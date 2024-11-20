using System.ComponentModel.DataAnnotations;

namespace BTL_WEBBH.Models.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file) {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "webp", "jpeg" ,"jfif"};

                bool result = extensions.Any(x => extension.EndsWith(x));

                if (!result) {
                    return new ValidationResult("AlLowed extensions are jpg, png, webp or jpeg");
                }
            }
            return ValidationResult.Success;
            
        }
    }
}
