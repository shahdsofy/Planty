using Blog_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Planty.Data.Configure
{
    public class ProfileConfigure:IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(x => new { x.UserID, x.ProfileNumber });
           
        }
    }
}
