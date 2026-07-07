using TaskFlow.Api.Contracts;
using TaskFlow.Application.Favorites;

namespace TaskFlow.Api.Endpoints;

/// <summary>
/// Favori HTTP route'ları — tam CRUD referansı (owned veri + EF).
/// POST 201 Created, GET 200, DELETE 204/404, duplicate 409.
/// </summary>
public static class FavoritesEndpoints
{
    public static IEndpointRouteBuilder MapFavoriteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/favorites").WithTags("Favorites");

        // GET /api/favorites?module=meals
        group.MapGet("", async (string module, IFavoriteService service, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(module))
            {
                return Results.BadRequest(new { title = "module parametresi zorunlu" });
            }

            var favorites = await service.GetByModuleAsync(module, ct);
            return Results.Ok(favorites);
        });

        // POST /api/favorites
        group.MapPost("", async (CreateFavoriteRequest request, IFavoriteService service, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(request.Module) ||
                string.IsNullOrWhiteSpace(request.ExternalId) ||
                string.IsNullOrWhiteSpace(request.Title))
            {
                return Results.BadRequest(new { title = "Module, ExternalId ve Title zorunlu" });
            }

            try
            {
                var created = await service.AddAsync(request, ct);
                return Results.Created($"/api/favorites/{created.Id}", created);
            }
            catch (DuplicateFavoriteException ex)
            {
                return Results.Conflict(new { title = ex.Message });
            }
        });

        // DELETE /api/favorites/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IFavoriteService service, CancellationToken ct) =>
        {
            var deleted = await service.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
