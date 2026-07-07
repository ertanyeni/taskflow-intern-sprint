using TaskFlow.Application.Ai;
using TaskFlow.Application.Meals;

namespace TaskFlow.Api.Endpoints;

/// <summary>
/// Meals HTTP route'ları. Endpoint sadece HTTP'yi bağlar; iş kuralı/cache MealService'te.
/// AI endpoint'i, AI'a özgü domain hatalarını anlamlı HTTP kodlarına çevirir (503/502).
/// </summary>
public static class MealsEndpoints
{
    public static IEndpointRouteBuilder MapMealEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/meals").WithTags("Meals");

        // GET /api/meals?search=&category=
        group.MapGet("", async (string? search, string? category, IMealService service, CancellationToken ct) =>
        {
            var meals = await service.GetMealsAsync(search, category, ct);
            return Results.Ok(meals);
        });

        // GET /api/meals/categories  (filtre kaynağı) — literal segment {id}'den önce eşleşir.
        group.MapGet("/categories", async (IMealService service, CancellationToken ct) =>
        {
            var categories = await service.GetCategoriesAsync(ct);
            return Results.Ok(categories);
        });

        // GET /api/meals/{id}
        group.MapGet("/{id}", async (string id, IMealService service, CancellationToken ct) =>
        {
            var meal = await service.GetMealAsync(id, ct);
            return meal is null ? Results.NotFound() : Results.Ok(meal);
        });

        // POST /api/meals/{id}/ai-summary
        group.MapPost("/{id}/ai-summary", async (string id, IMealService service, CancellationToken ct) =>
        {
            try
            {
                var summary = await service.GetAiSummaryAsync(id, ct);
                return Results.Ok(summary);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (AiNotConfiguredException ex)
            {
                // Key yok → 503: sunucu geçici olarak bu özelliği veremiyor.
                return Results.Problem(title: "AI yapılandırılmadı", detail: ex.Message, statusCode: 503);
            }
            catch (AiProviderException ex)
            {
                // Dış AI sağlayıcısı hata verdi → 502.
                return Results.Problem(title: "AI sağlayıcı hatası", detail: ex.Message, statusCode: 502);
            }
        });

        return app;
    }
}
