using Blog_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Blog_Platform.DTO
{
    public class UpdateTagDTO
    {
        [Required]
        [CheckOnIdValid<Tag>]
        public int Id { get; set; } //: Unique identifier for the tag.
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } //: Name of the tag.
    }

}
