using System.Net;
using System.Net.Http.Json;
using TaskFlow.Api.Contracts;
using Xunit;

namespace TaskFlow.Tests.Integration;

/// <summary>
/// Meals endpoint testleri — FakeMealDbClient + FakeAiService ile (canlı API'ye çıkılmaz).
/// Bu testler DB'ye dokunmaz (Meals dış veri, AI sahte).
/// </summary>
public class MealsEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public MealsEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_meals_returns_200_with_list()
    {
        var client = _factory.CreateClient();

        var meals = await client.GetFromJsonAsync<List<MealSummaryResponse>>("/api/meals");

        Assert.NotNull(meals);
        Assert.Equal(2, meals!.Count);
    }

    [Fact]
    public async Task Get_meals_with_unknown_search_returns_empty()
    {
        var client = _factory.CreateClient();

        var meals = await client.GetFromJsonAsync<List<MealSummaryResponse>>("/api/meals?search=bulunmayan");

        Assert.NotNull(meals);
        Assert.Empty(meals!);
    }

    [Fact]
    public async Task Get_categories_returns_200()
    {
        var client = _factory.CreateClient();

        var categories = await client.GetFromJsonAsync<List<MealCategoryResponse>>("/api/meals/categories");

        Assert.NotNull(categories);
        Assert.Equal(2, categories!.Count);
    }

    [Fact]
    public async Task Get_meal_by_id_returns_detail()
    {
        var client = _factory.CreateClient();

        var meal = await client.GetFromJsonAsync<MealDetailResponse>("/api/meals/52772");

        Assert.NotNull(meal);
        Assert.Equal("Teriyaki Chicken Casserole", meal!.Name);
        Assert.Equal(3, meal.Ingredients.Count);
    }

    [Fact]
    public async Task Get_unknown_meal_returns_404()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/meals/00000");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Ai_summary_returns_200_from_fake()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/meals/52772/ai-summary", null);
        response.EnsureSuccessStatusCode();
        var summary = await response.Content.ReadFromJsonAsync<AiSummaryResponse>();

        Assert.NotNull(summary);
        Assert.Contains("test", summary!.Tags);
        Assert.Equal("fake-model", summary.Model);
    }

    [Fact]
    public async Task Ai_summary_for_unknown_meal_returns_404()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/api/meals/00000/ai-summary", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
