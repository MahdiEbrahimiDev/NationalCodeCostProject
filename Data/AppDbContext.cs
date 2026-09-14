using Microsoft.EntityFrameworkCore;
using NationalCodeCostProject.Models;

namespace NationalCodeCostProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Cost> Costs { get; set; }
    public DbSet<User> Users { get; set; }
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cost>()
            .HasIndex(c => c.NationalCode)
            .IsUnique();
              modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
