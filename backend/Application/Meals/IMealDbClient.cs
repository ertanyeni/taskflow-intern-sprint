namespace TaskFlow.Application.Meals;

/// <summary>
/// Dış TheMealDB API'sinin soyutlaması. Neden arayüz? Çünkü integration testinde
/// canlı API'ye ÇIKMADAN bu arayüzü sahte (fake) bir implementasyonla değiştireceğiz.
/// HttpClient'ı doğrudan MealService içine gömseydik testi ucuza yazamazdık.
///
/// Somut implementasyon: Infrastructure/External/MealDb/MealDbClient.cs (typed HttpClient).
/// </summary>
public interface IMealDbClient
{
    Task<IReadOnlyList<MealSummary>> SearchByNameAsync(string? query, CancellationToken ct);

    Task<IReadOnlyList<MealSummary>> FilterByCategoryAsync(string category, CancellationToken ct);

    Task<MealDetail?> GetByIdAsync(string id, CancellationToken ct);

    Task<IReadOnlyList<MealCategory>> ListCategoriesAsync(CancellationToken ct);
}
