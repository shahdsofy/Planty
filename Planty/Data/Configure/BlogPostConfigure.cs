using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Platform.Data.Configure
{
    public class BlogPostConfigure : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).HasColumnName("PostId").HasColumnType("int");
            builder.Property(x=>x.Title).HasColumnName("Title").HasColumnType("varchar").HasMaxLength(100).IsRequired(true);
            builder.Property(x=>x.Content).HasColumnName("Content").HasColumnType("varchar").HasMaxLength(300).IsRequired(true);
            builder.Property(x=>x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("DateTime").IsRequired(true);
            builder.Property(x=>x.UpdatedDate).HasColumnName("UpdatedDate").HasColumnType("DateTime").IsRequired(true);
            builder.HasMany(x=>x.BlogPostHasTags).WithOne(x=>x.BlogPost).HasForeignKey(x=>x.PostId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.AppUser).WithMany(x=>x.posts).IsRequired(true).HasForeignKey(x=>x.AuthorId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x=>x.AuthorId).HasColumnName("AuthorId").IsRequired(true);
        }
    }
}
