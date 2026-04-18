using CleanCarApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanCarApi.Infrastructure.Data;

// Ärver nu från IdentityDbContext för att få användare och roller gratis
public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSet representerar en tabell i databasen
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Måste anropas för att Identity-tabellerna ska skapas korrekt
        base.OnModelCreating(modelBuilder);

        // Konfigurerar 1-till-många: en Brand har många Cars
        modelBuilder.Entity<Car>()
            .HasOne(c => c.Brand)
            .WithMany(b => b.Cars)
            .HasForeignKey(c => c.BrandId);

        // Price ska lagras med precision 18,2 (standard för pengar)
        modelBuilder.Entity<Car>()
            .Property(c => c.Price)
            .HasColumnType("decimal(18,2)");
    }
}