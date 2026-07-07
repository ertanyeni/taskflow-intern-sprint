using TaskFlow.Application.Meals;

namespace TaskFlow.Tests.Integration.Fakes;

/// <summary>
/// IMealDbClient'ın test sahtesi. Canlı TheMealDB'ye ÇIKMAZ; sabit veri döner.
/// Bu sınıf, IMealDbClient soyutlamasının VAROLUŞ SEBEBİDİR: gerçek HttpClient'ı
/// service'e gömseydik testte HttpMessageHandler mock'lamak zorunda kalırdık.
/// </summary>
public sealed class FakeMealDbClient : IMealDbClient
{
    public static readonly MealDetail Sample = new(
        Id: "52772",
        Name: "Teriyaki Chicken Casserole",
        Category: "Chicken",
        Area: "Japanese",
        Instructions: "Preheat oven. Cook rice. Combine and bake.",
        Thumbnail: "https://example.test/teriyaki.jpg",
        Youtube: "https://youtube.com/watch?v=test",
        Ingredients: ["2 cups Soy Sauce", "1 cup Water", "3 pieces Chicken"],
        Tags: ["Meat", "Casserole"]);

    public Task<IReadOnlyList<MealSummary>> SearchByNameAsync(string? query, CancellationToken ct)
    {
        IReadOnlyList<MealSummary> meals =
        [
            new(Sample.Id, Sample.Name, Sample.Category, Sample.Area, Sample.Thumbnail),
            new("52773", "Honey Teriyaki Salmon", "Seafood", "Japanese", "https://example.test/salmon.jpg")
        ];

        // "boş sonuç" senaryosunu da test edebilmek için basit filtre.
        if (!string.IsNullOrWhiteSpace(query) && query.Equals("bulunmayan", StringComparison.OrdinalIgnoreCase))
        {
            meals = [];
        }

        return Task.FromResult(meals);
    }

    public Task<IReadOnlyList<MealSummary>> FilterByCategoryAsync(string category, CancellationToken ct)
    {
        IReadOnlyList<MealSummary> meals =
        [
            new(Sample.Id, Sample.Name, category, string.Empty, Sample.Thumbnail)
        ];
        return Task.FromResult(meals);
    }

    public Task<MealDetail?> GetByIdAsync(string id, CancellationToken ct)
        => Task.FromResult<MealDetail?>(id == Sample.Id ? Sample : null);

    public Task<IReadOnlyList<MealCategory>> ListCategoriesAsync(CancellationToken ct)
    {
        IReadOnlyList<MealCategory> categories =
        [
            new("Chicken", "https://example.test/chicken.png"),
            new("Seafood", "https://example.test/seafood.png")
        ];
        return Task.FromResult(categories);
    }
}
