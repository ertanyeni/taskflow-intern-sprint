using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TaskFlow.Tests.Integration;

/// <summary>
/// İskeletin çalıştığını garanti eden ilk integration testi.
/// WebApplicationFactory tüm API host'unu bellek içinde ayağa kaldırır (DB gerektirmez, /health veri katmanına dokunmaz).
/// Stajyerler CreateTask/UpdateTaskStatus testlerini bu şablona bakarak yazar.
/// </summary>
public class HealthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Health_endpoint_returns_200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
