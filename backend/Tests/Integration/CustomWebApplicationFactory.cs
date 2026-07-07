using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using TaskFlow.Application.Ai;
using TaskFlow.Application.Meals;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Tests.Integration.Fakes;

namespace TaskFlow.Tests.Integration;

/// <summary>
/// Tüm API host'unu bellek içinde ayağa kaldırır ve dış bağımlılıkları (TheMealDB, Groq)
/// SAHTE implementasyonlarla değiştirir. Böylece testler canlı ağa hiç çıkmaz.
///
/// Anahtar desen: AddHttpClient ile kaydedilen IMealDbClient/IAiService,
/// ConfigureTestServices içinde RemoveAll + AddSingleton ile ezilir (sonradan eklenen kazanır).
///
/// Not: Postgres bağlantısı gerçek kalır (favori testleri DB'ye yazar). Bağlantı dizesi
/// appsettings'ten veya ConnectionStrings__Postgres env değişkeninden okunur.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IMealDbClient>();
            services.AddSingleton<IMealDbClient, FakeMealDbClient>();

            services.RemoveAll<IAiService>();
            services.AddSingleton<IAiService, FakeAiService>();
        });
    }

    // Migration process başına BİR KEZ çalışır. xUnit test sınıflarını paralel koşturur;
    // iki factory instance'ı aynı anda Migrate() çağırırsa taze DB'de CREATE'ler çakışır
    // (Postgres 23505 pg_type). Statik kilit + bayrak bunu serileştirir.
    private static readonly object MigrateLock = new();
    private static bool _migrated;

    // Host kurulunca şemayı garanti et: favori testleri gerçek Postgres'e yazar.
    // Böylece CI'daki taze postgres service container'ı da otomatik migrate olur.
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        lock (MigrateLock)
        {
            if (!_migrated)
            {
                using var scope = host.Services.CreateScope();
                scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
                _migrated = true;
            }
        }
        return host;
    }
}
