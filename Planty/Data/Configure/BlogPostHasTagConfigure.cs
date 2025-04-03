using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Platform.Data.Configure
{
    public class BlogPostHasTagConfigure : IEntityTypeConfiguration<BlogPostHasTag>
    {
        public void Configure(EntityTypeBuilder<BlogPostHasTag> builder)
        {
            builder.HasKey(x => new { x.TagId,x.PostId });
            builder.Property(x=>x.TagId).HasColumnName("TagId").HasColumnType("int");
            builder.Property(x=>x.PostId).HasColumnName("PostId").HasColumnType("int");
        }
    }
}
