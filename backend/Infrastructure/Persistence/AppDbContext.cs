using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Favorites;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext. PostgreSQL erişiminin tek kapısı.
/// Endpoint'ler doğrudan buraya erişmez; Application service üzerinden geçer.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    public DbSet<FavoriteItem> Favorites => Set<FavoriteItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Status).HasConversion<int>();
            entity.Property(t => t.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<FavoriteItem>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Module).IsRequired().HasMaxLength(50);
            entity.Property(f => f.ExternalId).IsRequired().HasMaxLength(100);
            entity.Property(f => f.Title).IsRequired().HasMaxLength(300);
            entity.Property(f => f.CreatedAt).IsRequired();
            // Aynı modülde aynı dış kayıt iki kez favorilenemez.
            entity.HasIndex(f => new { f.Module, f.ExternalId }).IsUnique();
        });
    }
}
