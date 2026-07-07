using Microsoft.Extensions.Caching.Memory;
using TaskFlow.Api.Contracts;
using TaskFlow.Application.Ai;

namespace TaskFlow.Application.Meals;

/// <summary>
/// Meals use-case orkestrasyonu: dış istemci (IMealDbClient) + cache (IMemoryCache) + AI (IAiService).
/// Cache stratejisi burada (endpoint'te değil). Read-model → Contract map burada.
///
/// TTL mantığı: Meals sabit referans veri → uzun TTL, dış API'ye gidiş minimize.
/// </summary>
public sealed class MealService : IMealService
{
    private readonly IMealDbClient _client;
    private readonly IMemoryCache _cache;
    private readonly IAiService _ai;

    private static readonly TimeSpan SearchTtl = TimeSpan.FromHours(1);
    private static readonly TimeSpan CategoryFilterTtl = TimeSpan.FromHours(6);
    private static readonly TimeSpan CategoriesTtl = TimeSpan.FromHours(24);
    private static readonly TimeSpan DetailTtl = TimeSpan.FromHours(12);
    private static readonly TimeSpan AiTtl = TimeSpan.FromHours(24);

    public MealService(IMealDbClient client, IMemoryCache cache, IAiService ai)
    {
        _client = client;
        _cache = cache;
        _ai = ai;
    }

    public async Task<IReadOnlyList<MealSummaryResponse>> GetMealsAsync(string? search, string? category, CancellationToken ct)
    {
        // Kategori filtresi aramaya göre önceliklidir (TheMealDB ikisini tek çağrıda desteklemez).
        if (!string.IsNullOrWhiteSpace(category))
        {
            var key = $"meals:filter:{category.ToLowerInvariant()}";
            var filtered = await _cache.GetOrCreateAsync(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CategoryFilterTtl;
                return _client.FilterByCategoryAsync(category, ct);
            });
            return MapSummaries(filtered);
        }

        var searchKey = $"meals:search:{(search ?? string.Empty).ToLowerInvariant()}";
        var meals = await _cache.GetOrCreateAsync(searchKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = SearchTtl;
            return _client.SearchByNameAsync(search, ct);
        });
        return MapSummaries(meals);
    }

    public async Task<IReadOnlyList<MealCategoryResponse>> GetCategoriesAsync(CancellationToken ct)
    {
        var categories = await _cache.GetOrCreateAsync("meals:categories", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CategoriesTtl;
            return _client.ListCategoriesAsync(ct);
        });

        return (categories ?? [])
            .Select(c => new MealCategoryResponse(c.Name, c.Thumbnail))
            .ToList();
    }

    public async Task<MealDetailResponse?> GetMealAsync(string id, CancellationToken ct)
    {
        var detail = await _cache.GetOrCreateAsync($"meals:detail:{id}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = DetailTtl;
            return _client.GetByIdAsync(id, ct);
        });

        if (detail is null)
        {
            return null;
        }

        return new MealDetailResponse(
            detail.Id,
            detail.Name,
            detail.Category,
            detail.Area,
            detail.Instructions,
            detail.Thumbnail,
            detail.Youtube,
            detail.Ingredients,
            detail.Tags);
    }

    public async Task<AiSummaryResponse> GetAiSummaryAsync(string id, CancellationToken ct)
    {
        var cacheKey = $"ai:meal:{id}:v1";
        if (_cache.TryGetValue(cacheKey, out AiResult? cached) && cached is not null)
        {
            return new AiSummaryResponse(cached.Summary, cached.Tags, cached.Model, Cached: true);
        }

        var meal = await GetMealAsync(id, ct)
            ?? throw new KeyNotFoundException($"Meal bulunamadı: {id}");

        var content = $"{meal.Name} ({meal.Area}, {meal.Category}). Talimatlar: {meal.Instructions}";
        var result = await _ai.SummarizeAndTagAsync(meal.Name, content, ct);

        _cache.Set(cacheKey, result, AiTtl);
        return new AiSummaryResponse(result.Summary, result.Tags, result.Model, Cached: false);
    }

    private static IReadOnlyList<MealSummaryResponse> MapSummaries(IReadOnlyList<MealSummary>? meals)
        => (meals ?? [])
            .Select(m => new MealSummaryResponse(m.Id, m.Name, m.Category, m.Area, m.Thumbnail))
            .ToList();
}
