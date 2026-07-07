using TaskFlow.Api.Contracts;

namespace TaskFlow.Application.Meals;

/// <summary>
/// Meals use-case'leri. Endpoint bu arayüzle konuşur. Somut sınıf (MealService)
/// dış istemci + cache + AI'ı orkestrasyonla birleştirir.
/// </summary>
public interface IMealService
{
    Task<IReadOnlyList<MealSummaryResponse>> GetMealsAsync(string? search, string? category, CancellationToken ct);

    Task<IReadOnlyList<MealCategoryResponse>> GetCategoriesAsync(CancellationToken ct);

    Task<MealDetailResponse?> GetMealAsync(string id, CancellationToken ct);

    Task<AiSummaryResponse> GetAiSummaryAsync(string id, CancellationToken ct);
}
