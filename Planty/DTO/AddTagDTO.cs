using Blog_Platform.IRepository;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class AddTagDTO
    {
        [Required]
        [MaxLength(50)]
        [CheckTagExistBefore]
        public string Name { get; set; } //: Name of the tag.
    }

    internal class CheckTagExistBeforeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
                return null;
            ITagRepo? tagRepo = validationContext.GetService<ITagRepo>();
            if (tagRepo is null)
                return new ValidationResult("can't Provide The needed Service");
            if(tagRepo.CheckNameExistBefore(value.ToString()!))
                return ValidationResult.Success;
            return new ValidationResult("This Tag Exist Before");
        }
    }
}
