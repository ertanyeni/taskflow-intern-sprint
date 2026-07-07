namespace TaskFlow.Api.Contracts;

// API'nin dışarıya döndüğü sözleşmeler. İç read-model (Application/Meals) değişse bile
// bu sözleşme kararlı kalır. Tasks'taki TaskResponse.cs ile aynı fikir.

public record MealSummaryResponse(
    string Id,
    string Name,
    string Category,
    string Area,
    string Thumbnail);

public record MealDetailResponse(
    string Id,
    string Name,
    string Category,
    string Area,
    string Instructions,
    string Thumbnail,
    string? Youtube,
    IReadOnlyList<string> Ingredients,
    IReadOnlyList<string> Tags);

public record MealCategoryResponse(
    string Name,
    string? Thumbnail);

public record AiSummaryResponse(
    string Summary,
    IReadOnlyList<string> Tags,
    string Model,
    bool Cached);
