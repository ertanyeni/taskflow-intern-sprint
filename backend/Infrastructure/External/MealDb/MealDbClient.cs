using System.Text.Json;
using TaskFlow.Application.Meals;

namespace TaskFlow.Infrastructure.External.MealDb;

/// <summary>
/// IMealDbClient'ın somut implementasyonu. "Typed HttpClient": HttpClient constructor'dan
/// enjekte edilir (Program.cs'te AddHttpClient ile kaydedilir, BaseAddress orada set edilir).
/// Tek sorumluluğu: HTTP çağrısı yap + ham JSON'u temiz read-model'e çevir.
/// Cache YOK (o MealService'in işi), iş kuralı YOK.
/// </summary>
public sealed class MealDbClient : IMealDbClient
{
    private readonly HttpClient _http;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public MealDbClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<IReadOnlyList<MealSummary>> SearchByNameAsync(string? query, CancellationToken ct)
    {
        // Boş arama = "a" ile başlayan bir varsayılan liste (TheMealDB search.php?s= boşsa da veri döner).
        var url = $"search.php?s={Uri.EscapeDataString(query ?? string.Empty)}";
        var raw = await GetAsync<MealDbSearchResponse>(url, ct);
        return MapSummaries(raw?.Meals);
    }

    public async Task<IReadOnlyList<MealSummary>> FilterByCategoryAsync(string category, CancellationToken ct)
    {
        var url = $"filter.php?c={Uri.EscapeDataString(category)}";
        var raw = await GetAsync<MealDbSearchResponse>(url, ct);
        // filter.php sadece id/isim/thumb döner; kategori/area boş gelir → istenen kategoriyi biz koyarız.
        return (raw?.Meals ?? [])
            .Where(m => m.IdMeal is not null && m.StrMeal is not null)
            .Select(m => new MealSummary(m.IdMeal!, m.StrMeal!, category, string.Empty, m.StrMealThumb ?? string.Empty))
            .ToList();
    }

    public async Task<MealDetail?> GetByIdAsync(string id, CancellationToken ct)
    {
        var url = $"lookup.php?i={Uri.EscapeDataString(id)}";
        var raw = await GetAsync<MealDbSearchResponse>(url, ct);
        var meal = raw?.Meals?.FirstOrDefault();
        if (meal?.IdMeal is null || meal.StrMeal is null)
        {
            return null;
        }

        return new MealDetail(
            meal.IdMeal,
            meal.StrMeal,
            meal.StrCategory ?? string.Empty,
            meal.StrArea ?? string.Empty,
            meal.StrInstructions ?? string.Empty,
            meal.StrMealThumb ?? string.Empty,
            NormalizeYoutube(meal.StrYoutube),
            ExtractIngredients(meal),
            ExtractTags(meal.StrTags));
    }

    public async Task<IReadOnlyList<MealCategory>> ListCategoriesAsync(CancellationToken ct)
    {
        var raw = await GetAsync<MealDbCategoriesResponse>("categories.php", ct);
        return (raw?.Categories ?? [])
            .Where(c => !string.IsNullOrWhiteSpace(c.StrCategory))
            .Select(c => new MealCategory(c.StrCategory!, c.StrCategoryThumb))
            .ToList();
    }

    private async Task<T?> GetAsync<T>(string url, CancellationToken ct)
    {
        using var response = await _http.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, ct);
    }

    private static IReadOnlyList<MealSummary> MapSummaries(IEnumerable<MealDbMealRaw>? meals)
    {
        return (meals ?? [])
            .Where(m => m.IdMeal is not null && m.StrMeal is not null)
            .Select(m => new MealSummary(
                m.IdMeal!,
                m.StrMeal!,
                m.StrCategory ?? string.Empty,
                m.StrArea ?? string.Empty,
                m.StrMealThumb ?? string.Empty))
            .ToList();
    }

    private static string? NormalizeYoutube(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    private static IReadOnlyList<string> ExtractTags(string? tags)
        => string.IsNullOrWhiteSpace(tags)
            ? []
            : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    // strIngredient1..20 + strMeasure1..20 → "2 cups Flour" gibi birleşik satırlar.
    private static IReadOnlyList<string> ExtractIngredients(MealDbMealRaw meal)
    {
        var result = new List<string>();
        if (meal.Extra is null)
        {
            return result;
        }

        for (var i = 1; i <= 20; i++)
        {
            var ingredient = ReadString(meal.Extra, $"strIngredient{i}");
            if (string.IsNullOrWhiteSpace(ingredient))
            {
                continue;
            }

            var measure = ReadString(meal.Extra, $"strMeasure{i}");
            result.Add(string.IsNullOrWhiteSpace(measure)
                ? ingredient.Trim()
                : $"{measure.Trim()} {ingredient.Trim()}");
        }

        return result;
    }

    private static string? ReadString(Dictionary<string, JsonElement> data, string key)
        => data.TryGetValue(key, out var el) && el.ValueKind == JsonValueKind.String
            ? el.GetString()
            : null;
}
