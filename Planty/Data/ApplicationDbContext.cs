using Blog_Platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Planty.Models;
using System;

namespace Planty.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		#region Tables
		//public DbSet<User> Users { get; set; }
		//public DbSet<Admin> Admins { get; set; }
		
		
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Plant> Plants { get; set; }
		public DbSet<Order> Orders { get; set; }
		//public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Stock> Stocks { get; set; }



        public DbSet<BlogPost> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<Revoke> Revoke { get; set; }
        public DbSet<Token> Tokens { get; set; }
        //	public DbSet<UsersOrders> UsersOrders { get; set; }
        #endregion
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(builder);
        }
       

		
	}
}
