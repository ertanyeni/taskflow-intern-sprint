using System.Text.Json.Serialization;

namespace TaskFlow.Infrastructure.External.MealDb;

// TheMealDB'nin HAM JSON şekilleri. Bilerek 'internal': bu çirkin, provider'a özgü
// şekil (strMeal, strIngredient1..20 ...) dış dünyaya ASLA sızmaz. MealDbClient bunları
// temiz read-model'e (MealSummary/MealDetail) çevirir.
//
// Örnek: https://www.themealdb.com/api/json/v1/1/lookup.php?i=52772

internal sealed class MealDbSearchResponse
{
    [JsonPropertyName("meals")]
    public List<MealDbMealRaw>? Meals { get; set; }
}

internal sealed class MealDbCategoriesResponse
{
    [JsonPropertyName("categories")]
    public List<MealDbCategoryRaw>? Categories { get; set; }
}

internal sealed class MealDbCategoryRaw
{
    [JsonPropertyName("strCategory")]
    public string? StrCategory { get; set; }

    [JsonPropertyName("strCategoryThumb")]
    public string? StrCategoryThumb { get; set; }
}

internal sealed class MealDbMealRaw
{
    [JsonPropertyName("idMeal")]
    public string? IdMeal { get; set; }

    [JsonPropertyName("strMeal")]
    public string? StrMeal { get; set; }

    [JsonPropertyName("strCategory")]
    public string? StrCategory { get; set; }

    [JsonPropertyName("strArea")]
    public string? StrArea { get; set; }

    [JsonPropertyName("strInstructions")]
    public string? StrInstructions { get; set; }

    [JsonPropertyName("strMealThumb")]
    public string? StrMealThumb { get; set; }

    [JsonPropertyName("strYoutube")]
    public string? StrYoutube { get; set; }

    [JsonPropertyName("strTags")]
    public string? StrTags { get; set; }

    // TheMealDB malzemeleri strIngredient1..strIngredient20 olarak DÜZ tutar.
    // Bu sözlük, bilinmeyen JSON alanlarını (strIngredientN, strMeasureN) yakalar.
    [JsonExtensionData]
    public Dictionary<string, System.Text.Json.JsonElement>? Extra { get; set; }
}
