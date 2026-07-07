using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Tasks;

namespace TaskFlow.Infrastructure.Persistence;

/// <summary>
/// Development'ta Tasks tablosu boşsa birkaç demo görev ekler. Böylece /tasks sayfası
/// boş görünmez; Stajyer 2 rung 1-2'de (filtre + sayaç, türetilmiş alan) üzerinde çalışacak
/// gerçek veri bulur. Idempotent: veri varsa hiçbir şey yapmaz.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedDevelopmentAsync(AppDbContext db, CancellationToken ct = default)
    {
        if (await db.Tasks.AnyAsync(ct))
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;
        db.Tasks.AddRange(
            new TaskItem { Id = Guid.NewGuid(), Title = "Repo'yu klonla ve çalıştır",        Status = TaskItemStatus.Done,       CreatedAt = now.AddDays(-6) },
            new TaskItem { Id = Guid.NewGuid(), Title = "Gold Meals modülünü incele",         Status = TaskItemStatus.Done,       CreatedAt = now.AddDays(-4) },
            new TaskItem { Id = Guid.NewGuid(), Title = "İlk issue'yu al ve branch aç",       Status = TaskItemStatus.InProgress, CreatedAt = now.AddDays(-2) },
            new TaskItem { Id = Guid.NewGuid(), Title = "Küçük PR aç, CI'ı yeşile getir",     Status = TaskItemStatus.InProgress, CreatedAt = now.AddDays(-1) },
            new TaskItem { Id = Guid.NewGuid(), Title = "Veri akışını çizerek anlat",         Status = TaskItemStatus.Todo,       CreatedAt = now.AddHours(-5) },
            new TaskItem { Id = Guid.NewGuid(), Title = "Rung 2: türetilmiş alanı ekle",      Status = TaskItemStatus.Todo,       CreatedAt = now.AddHours(-2) }
        );

        await db.SaveChangesAsync(ct);
    }
}
