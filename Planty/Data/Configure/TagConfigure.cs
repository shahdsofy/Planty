using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Platform.Data.Configure
{
    public class TagConfigure : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).HasColumnName("TagId").HasColumnType("int");
            builder.Property(x=>x.Name).HasColumnName("Name").HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
            builder.HasIndex(x => x.Name).IsUnique(true);
            builder.HasMany(x=>x.BlogPostHasTags).WithOne(x=>x.Tag).IsRequired(true).HasForeignKey(x=>x.TagId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
