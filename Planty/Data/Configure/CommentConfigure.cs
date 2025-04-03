using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Platform.Data.Configure
{
    public class CommentConfigure : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).HasColumnName("CommentId").HasColumnType("int");
            builder.Property(x=>x.PostId).HasColumnName("PostId").HasColumnType("int");
            builder.Property(x=>x.Content).HasColumnName("Content").HasColumnType("varchar").HasMaxLength(300).IsRequired(true);
            builder.Property(x=>x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("DateTime").IsRequired(true);
            builder.Property(x=>x.UpdatedDate).HasColumnName("UpdatedDate").HasColumnType("DateTime").IsRequired(true);
            builder.HasOne(x=>x.AppUser).WithMany(x=>x.Comments).IsRequired(true).HasForeignKey(x=>x.AuthorId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.BlogPost).WithMany(x=>x.Comments).HasForeignKey(x=>x.PostId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x=>x.AuthorId).HasColumnName("AuthorId").IsRequired(true);
        }
    }
}
