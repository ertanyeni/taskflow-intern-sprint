using System.Net;
using System.Net.Http.Json;
using TaskFlow.Api.Contracts;
using Xunit;

namespace TaskFlow.Tests.Integration;

/// <summary>
/// Favori endpoint testleri — GERÇEK Postgres'e yazar (owned veri). CI'da postgres:16
/// service container gerekir; local'de ConnectionStrings__Postgres env ile çalıştır.
/// Çakışmayı önlemek için her test benzersiz ExternalId (Guid) kullanır.
/// </summary>
public class FavoritesEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public FavoritesEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private static CreateFavoriteRequest NewFavorite() => new(
        Module: "meals",
        ExternalId: Guid.NewGuid().ToString("N"),
        Title: "Test Meal",
        Thumbnail: null);

    [Fact]
    public async Task Post_favorite_returns_201_and_get_lists_it()
    {
        var client = _factory.CreateClient();
        var request = NewFavorite();

        var postResponse = await client.PostAsJsonAsync("/api/favorites", request);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var created = await postResponse.Content.ReadFromJsonAsync<FavoriteResponse>();
        Assert.NotNull(created);
        Assert.Equal(request.ExternalId, created!.ExternalId);

        var list = await client.GetFromJsonAsync<List<FavoriteResponse>>("/api/favorites?module=meals");
        Assert.Contains(list!, f => f.Id == created.Id);
    }

    [Fact]
    public async Task Post_duplicate_favorite_returns_409()
    {
        var client = _factory.CreateClient();
        var request = NewFavorite();

        var first = await client.PostAsJsonAsync("/api/favorites", request);
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);

        var second = await client.PostAsJsonAsync("/api/favorites", request);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }

    [Fact]
    public async Task Post_favorite_missing_fields_returns_400()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/favorites",
            new CreateFavoriteRequest(Module: "", ExternalId: "", Title: "", Thumbnail: null));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_existing_favorite_returns_204()
    {
        var client = _factory.CreateClient();
        var created = await (await client.PostAsJsonAsync("/api/favorites", NewFavorite()))
            .Content.ReadFromJsonAsync<FavoriteResponse>();

        var delete = await client.DeleteAsync($"/api/favorites/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);
    }

    [Fact]
    public async Task Delete_unknown_favorite_returns_404()
    {
        var client = _factory.CreateClient();

        var delete = await client.DeleteAsync($"/api/favorites/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }
}
