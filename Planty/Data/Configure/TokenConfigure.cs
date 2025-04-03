using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog_Platform.Data.Configure
{
    public class TokenConfigure : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId).HasColumnName("UserId").ValueGeneratedNever();
            builder.Property(x => x.token).HasColumnName("token").IsRequired(true);
            builder.Property(x => x.IsRevoked).HasColumnName("IsRevoked").HasColumnType("BIT").IsRequired(true);
            builder.HasOne(x => x.User).WithOne(x => x.Token).IsRequired(true).HasForeignKey<Token>(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
