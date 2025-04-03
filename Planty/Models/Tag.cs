using Microsoft.EntityFrameworkCore;

namespace Blog_Platform.Models
{
    public class Tag : IModelHelper
    {
        public int Id { get; set; } //: Unique identifier for the tag.
        public string Name { get; set; } //: Name of the tag.
        public List<BlogPostHasTag> BlogPostHasTags { get; set; } = new List<BlogPostHasTag>();
    }
} 
