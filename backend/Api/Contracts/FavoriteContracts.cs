namespace TaskFlow.Api.Contracts;

public record CreateFavoriteRequest(
    string Module,
    string ExternalId,
    string Title,
    string? Thumbnail);

public record FavoriteResponse(
    Guid Id,
    string Module,
    string ExternalId,
    string Title,
    string? Thumbnail,
    DateTimeOffset CreatedAt);
