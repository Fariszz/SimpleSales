using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Backend.Data;

public class ApplicationDbContext : DbContext
{

    private readonly IConfiguration Configuration;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        Configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "ADMIN" },
            new Role { Id = 2, Name = "SALES" }
        );
        
        builder.Entity<User>()
            .HasOne<Role>(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
        
        

        builder.Entity<User>().HasData(
            new User
            {
                Id = 1, 
                Name = "Admin",
                Email = "admin@admin.com",
                Password = HashPassword("admin123"),
                RoleId = 1,
                Created = DateTime.Now
            },
            new User
            {
                Id = 2, 
                Name = "Sales",
                Email = "sales@sales.com",
                Password = HashPassword("sales123"),
                RoleId = 2,
                Created = DateTime.Now
            });
    }
    
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    
    // public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}