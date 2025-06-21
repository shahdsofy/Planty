using Blog_Platform.IRepository;
using Blog_Platform;
using Blog_Platform.Models;
using Blog_Platform.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Planty.Data;
using Planty.Models;
using System.Text;
using Planty.Repositories.Interfaces;
using Planty.Repositories;
using Planty.Services.Interfaces;
using Planty.Services;
using Planty.IRepository;
using Planty.Repository;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
// Planty.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("pp"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{
    option.SignIn.RequireConfirmedEmail = true;
    option.Password.RequiredLength = 8;
	option.Password.RequireNonAlphanumeric = false;
	option.Password.RequireUppercase = false;
	option.Password.RequireLowercase = false;
	option.Password.RequireDigit = false;
}).AddEntityFrameworkStores<ApplicationDbContext>(); 

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.SaveToken = true;
	options.RequireHttpsMetadata = false;
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration["JWT:issuer"],
		ValidateAudience = true,
		ValidAudience = builder.Configuration["JWT:audience"],
		IssuerSigningKey =
			new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
	};
});
builder.Services.AddScoped<IBlogPostRepo, BlogPostRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();

builder.Services.AddTransient<ITokenRepo, TokenRepo>();
builder.Services.AddTransient<RevokeMiddleWare>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
	swagger.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "ASP.NET 8 Web API",
		Description = "ITI Project"
	});

	swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
	});
	swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] {}
		}
	});
});

var app = builder.Build();

// Create roles on app startup
app.Lifetime.ApplicationStarted.Register(async () =>
{
	using var scope = app.Services.CreateScope();
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

	string[] roles = new[] { "ADMIN", "AUTHOR", "USER" };

	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}
	//allow use static files
	app.UseStaticFiles();

});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "posts")),
    RequestPath = "/posts"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","plants")),
    RequestPath = "/images/plants"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profile_picture")),
    RequestPath = "/profile_picture"
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
