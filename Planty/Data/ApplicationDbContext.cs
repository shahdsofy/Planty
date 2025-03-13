using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Planty.Models;

namespace Planty.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		#region Tables
		//public DbSet<User> Users { get; set; }
		//public DbSet<Admin> Admins { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Plant> Plants { get; set; }
		public DbSet<Order> Orders { get; set; }
		//public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Stock> Stocks { get; set; }
		//	public DbSet<UsersOrders> UsersOrders { get; set; }
		#endregion
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{


			//modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

			//modelBuilder.Entity<User>().Property(u => u.Email).HasAnnotation("RegularExpression", @"^[a-zA-Z0-9._%+-]+@gmail\.com$");

			//modelBuilder.Entity<User>().Property(u => u.Password).HasAnnotation("RegularExpression", @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$");

			// Profile one-to-one relationship
			modelBuilder.Entity<Profile>()
			   .HasKey(p => new { p.UserID, p.ProfileNumber });
			//modelBuilder.Entity<Profile>()
			//	.HasOne(p => p.User)
			//	.WithOne(u => u.Profile)
			//	.HasForeignKey<Profile>(p => p.UserID)
			//	.OnDelete(DeleteBehavior.Cascade);

			// Post one-to-many relationship with User
			//modelBuilder.Entity<Post>()
			//	.HasOne(p => p.User)
			//	.WithMany(u => u.Posts)
			//	.HasForeignKey(p => p.UserID)
			//	.OnDelete(DeleteBehavior.Cascade);

			// Comment one-to-many relationship with Post
			modelBuilder.Entity<Comment>()
				.HasOne(c => c.Post)
				.WithMany(p => p.Comments)
				.HasForeignKey(c => c.PostID)
				.OnDelete(DeleteBehavior.Cascade);

			// Cart one-to-one relationship with User
			//modelBuilder.Entity<Cart>()
			//	.HasOne(c => c.User)
			//	.WithOne(u => u.Cart)
			//	.HasForeignKey<Cart>(c => c.UserID)
			//	.OnDelete(DeleteBehavior.Cascade);

			// CartItem many-to-one relationship with Cart
			modelBuilder.Entity<CartItem>()
				.HasOne(ci => ci.Cart)
				.WithMany(c => c.CartItems)
				.HasForeignKey(ci => ci.CartID)
				.OnDelete(DeleteBehavior.Cascade);

			// Order many-to-one relationship with User
			//modelBuilder.Entity<Order>()
			//	.HasOne(o => o.User)
			//	.WithMany(u => u.Orders)
			//	.HasForeignKey(o => o.UserID)
			//	.OnDelete(DeleteBehavior.Cascade);

			// OrderItem many-to-one relationship with Order
			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Order)
				.WithMany(o => o.OrderItems)
				.HasForeignKey(oi => oi.OrderID)
				.OnDelete(DeleteBehavior.Cascade);

			// Stock weak entity with composite key
			//modelBuilder.Entity<Stock>()
			//	.HasKey(s => new { s.PlantID, s.AdminID });

			//modelBuilder.Entity<Stock>()
			//	.HasOne(s => s.Plant)
			//	.WithMany()
			//	.HasForeignKey(s => s.PlantID);

			//modelBuilder.Entity<Stock>()
			//	.HasOne(s => s.Admin)
			//	.WithMany(a => a.ManagedStocks)
			//	.HasForeignKey(s => s.AdminID);


			base.OnModelCreating(modelBuilder);


		}
	}
}
