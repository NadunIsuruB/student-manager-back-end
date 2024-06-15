using Microsoft.EntityFrameworkCore;
using StudentManager.Models.Dtos;

namespace StudentManager;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    // Entities
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().Property(u => u.Name).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
        
        modelBuilder.Entity<Student>().HasKey(s => s.Id);
        modelBuilder.Entity<Student>().HasIndex(s => s.NIC).IsUnique();
        modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();
        modelBuilder.Entity<Student>().Property(s => s.FirstName).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.LastName).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.Mobile).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.Email).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.NIC).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.DateOfBirth).IsRequired();
        modelBuilder.Entity<Student>().Property(s => s.Address).IsRequired();    
    }
}