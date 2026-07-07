using TaskFlow.Api.Contracts;

namespace TaskFlow.Application.Favorites;

public interface IFavoriteService
{
    Task<IReadOnlyList<FavoriteResponse>> GetByModuleAsync(string module, CancellationToken ct);

    /// <exception cref="DuplicateFavoriteException">Aynı (Module, ExternalId) zaten varsa.</exception>
    Task<FavoriteResponse> AddAsync(CreateFavoriteRequest request, CancellationToken ct);

    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}

/// <summary>Aynı favori tekrar eklenmeye çalışılırsa → endpoint 409'a çevirir.</summary>
public sealed class DuplicateFavoriteException : Exception
{
    public DuplicateFavoriteException(string message) : base(message) { }
}
