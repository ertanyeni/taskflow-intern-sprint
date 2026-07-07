using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Contracts;
using TaskFlow.Domain.Favorites;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Application.Favorites;

/// <summary>
/// Favori use-case'leri. EF Core ile Postgres'e yazar/okur. Tasks'taki TaskService ile
/// aynı desen: DbContext enjekte, entity ↔ Contract map, iş kuralı (duplicate) burada.
/// </summary>
public sealed class FavoriteService : IFavoriteService
{
    private readonly AppDbContext _db;

    public FavoriteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<FavoriteResponse>> GetByModuleAsync(string module, CancellationToken ct)
    {
        return await _db.Favorites
            .Where(f => f.Module == module)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FavoriteResponse(f.Id, f.Module, f.ExternalId, f.Title, f.Thumbnail, f.CreatedAt))
            .ToListAsync(ct);
    }

    public async Task<FavoriteResponse> AddAsync(CreateFavoriteRequest request, CancellationToken ct)
    {
        var exists = await _db.Favorites
            .AnyAsync(f => f.Module == request.Module && f.ExternalId == request.ExternalId, ct);
        if (exists)
        {
            throw new DuplicateFavoriteException(
                $"Bu favori zaten var: {request.Module}/{request.ExternalId}");
        }

        var favorite = new FavoriteItem
        {
            Id = Guid.NewGuid(),
            Module = request.Module,
            ExternalId = request.ExternalId,
            Title = request.Title,
            Thumbnail = request.Thumbnail,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.Favorites.Add(favorite);
        await _db.SaveChangesAsync(ct);

        return new FavoriteResponse(
            favorite.Id, favorite.Module, favorite.ExternalId, favorite.Title, favorite.Thumbnail, favorite.CreatedAt);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var favorite = await _db.Favorites.FirstOrDefaultAsync(f => f.Id == id, ct);
        if (favorite is null)
        {
            return false;
        }

        _db.Favorites.Remove(favorite);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
