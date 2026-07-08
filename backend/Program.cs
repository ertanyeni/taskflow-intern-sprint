using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskFlow.Api.Endpoints;
using TaskFlow.Api.Middleware;
using TaskFlow.Application.Ai;
using TaskFlow.Application.Favorites;
using TaskFlow.Application.Meals;
using TaskFlow.Application.Tasks;
using TaskFlow.Infrastructure.External.Groq;
using TaskFlow.Infrastructure.External.MealDb;
using TaskFlow.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// --- CORS: frontend dev origin'lerine izin ver ---
const string CorsPolicy = "frontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:4173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// --- EF Core + PostgreSQL ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// --- Cache (dış API sonuçlarını tutmak için) ---
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IMealDbClient, MealDbClient>(client =>
{
    client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
    client.Timeout = TimeSpan.FromSeconds(10);
});
// --- Meals modülü (gold referans): dış API typed HttpClient + application service ---
builder.Services.AddHttpClient<IMealDbClient, MealDbClient>(client =>
{
    client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// --- Groq AI (OpenAI-uyumlu). Key server-side; user-secrets/env'den okunur. ---
builder.Services.Configure<GroqOptions>(builder.Configuration.GetSection(GroqOptions.SectionName));
builder.Services.AddHttpClient<IAiService, GroqAiService>((sp, client) =>
{
    var o = sp.GetRequiredService<IOptions<GroqOptions>>().Value;
    client.BaseAddress = new Uri(o.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient<IAiService, GroqAiService>((sp, client) =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<GroqOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// --- Application services ---
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IMealService, MealService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

// --- Swagger (sadece Development) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global hata middleware'i en dışta.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);

// Development'ta /tasks boş görünmesin diye demo veri ekle (idempotent).
if (app.Environment.IsDevelopment())
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DbSeeder.SeedDevelopmentAsync(db);
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "Demo veri seed'i atlandı (DB migrate edildi mi?).");
    }
}

app.MapHealthEndpoints();
app.MapTaskEndpoints();
app.MapMealEndpoints();
app.MapFavoriteEndpoints();

app.Run();

// Integration testlerinin WebApplicationFactory<Program> ile host'u ayağa kaldırabilmesi için.
public partial class Program { }
