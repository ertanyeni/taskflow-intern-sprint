namespace TaskFlow.Application.Meals;

// Bu tipler UYGULAMANIN İÇ read-model'leridir: dış TheMealDB JSON'undan map edilen
// temiz, sade tipler. Ne dış ham DTO'dur (o Infrastructure'da internal kalır) ne de
// API sözleşmesidir (o Api/Contracts'tadır). Öğretim noktası: sahip olmadığın veriyi
// Domain entity'si + migration yapma; onu read-model olarak modelle.

public record MealSummary(
    string Id,
    string Name,
    string Category,
    string Area,
    string Thumbnail);

public record MealDetail(
    string Id,
    string Name,
    string Category,
    string Area,
    string Instructions,
    string Thumbnail,
    string? Youtube,
    IReadOnlyList<string> Ingredients,
    IReadOnlyList<string> Tags);

public record MealCategory(
    string Name,
    string? Thumbnail);
