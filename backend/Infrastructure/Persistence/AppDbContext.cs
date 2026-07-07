using Microsoft.EntityFrameworkCore;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Status).HasConversion<int>();
            entity.Property(t => t.CreatedAt).IsRequired();
        });
    }
}
