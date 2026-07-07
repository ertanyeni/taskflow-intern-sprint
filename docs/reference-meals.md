# Gold Referans: Meals Modülü — Komut Komut Walkthrough

Bu doküman, `Meals` modülünün **nasıl** kurulduğunu adım adım, komutlarıyla anlatır.
Stajyerler kendi modüllerini (Pokédex / Kütüphane) kurarken **bu sırayı** izler.

> Kural: Önce backend dikey kesiti çalışsın (`curl` ile doğrula), sonra frontend'i bağla.
> Her adımda küçük commit at.

---

## 0. Ortamı hazırla
```bash
# Repo kökünde: PostgreSQL
docker compose up -d postgres            # 5432 doluysa: POSTGRES_PORT=5442 docker compose up -d postgres

cd backend
dotnet restore
dotnet tool install --global dotnet-ef   # bir kez
```

---

## 1. Dış veri = read-model (Domain entity DEĞİL)
Meals verisi bize ait değil → `Domain/` + migration YOK. İç read-model kullan.

Dosyalar:
- `Application/Meals/MealReadModels.cs` — `MealSummary`, `MealDetail`, `MealCategory` (temiz record'lar).
- `Application/Meals/IMealDbClient.cs` — dış API soyutlaması (test için şart).

## 2. Typed HttpClient (Infrastructure)
- `Infrastructure/External/MealDb/MealDbRawDtos.cs` — TheMealDB ham JSON (`strMeal`, `strIngredient1..20`), **`internal`** (dışarı sızmaz).
- `Infrastructure/External/MealDb/MealDbClient.cs` — `IMealDbClient` impl; enjekte `HttpClient`; ham → read-model map.

`Program.cs`'e ekle:
```csharp
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IMealDbClient, MealDbClient>(client =>
{
    client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
    client.Timeout = TimeSpan.FromSeconds(10);
});
```

## 3. Contract + Application service (cache burada)
- `Api/Contracts/MealResponses.cs` — dış sözleşme (`MealSummaryResponse` ...).
- `Application/Meals/IMealService.cs` + `MealService.cs` — `IMealDbClient` + `IMemoryCache` orkestrasyonu. TTL: categories 24h, detail 12h, filter 6h, search 1h.

`Program.cs`: `builder.Services.AddScoped<IMealService, MealService>();`

## 4. Endpoint'ler
- `Api/Endpoints/MealsEndpoints.cs` — `MapMealEndpoints()`:
  `GET /api/meals?search=&category=`, `/categories`, `/{id}` (404), `POST /{id}/ai-summary`.

`Program.cs`: `app.MapMealEndpoints();`

**Doğrula:**
```bash
export ConnectionStrings__Postgres="Host=localhost;Port=5442;Database=taskflow;Username=taskflow;Password=taskflow"  # port'unu kullan
dotnet run --urls http://localhost:5080
# başka terminalde:
curl "http://localhost:5080/api/meals/categories"
curl "http://localhost:5080/api/meals?search=chicken"
curl "http://localhost:5080/api/meals/52772"
```

## 5. Favoriler = owned veri → Domain entity + EF migration
- `Domain/Favorites/FavoriteItem.cs` — entity (`Module`, `ExternalId`, unique çift).
- `Infrastructure/Persistence/AppDbContext.cs` — `DbSet<FavoriteItem>` + `HasIndex(...).IsUnique()`.
- `Application/Favorites/*` + `Api/Endpoints/FavoritesEndpoints.cs` (POST 201 / GET / DELETE 204 / duplicate 409).

Migration:
```bash
dotnet ef migrations add AddFavorites
dotnet ef database update
```
`Program.cs`: `builder.Services.AddScoped<IFavoriteService, FavoriteService>();` + `app.MapFavoriteEndpoints();`

**Doğrula:**
```bash
curl -X POST http://localhost:5080/api/favorites \
  -H "Content-Type: application/json" \
  -d '{"module":"meals","externalId":"52772","title":"Teriyaki","thumbnail":null}'
curl "http://localhost:5080/api/favorites?module=meals"
```

## 6. AI = Groq (OpenAI-uyumlu), key server-side
- `Application/Ai/{IAiService, GroqOptions}.cs` — soyutlama + config.
- `Infrastructure/External/Groq/{GroqRawDtos, GroqAiService}.cs` — `chat/completions` çağrısı; key yoksa `AiNotConfiguredException` (→ 503), sağlayıcı hatası → 502.

`Program.cs`:
```csharp
builder.Services.Configure<GroqOptions>(builder.Configuration.GetSection(GroqOptions.SectionName));
builder.Services.AddHttpClient<IAiService, GroqAiService>((sp, client) =>
{
    var o = sp.GetRequiredService<IOptions<GroqOptions>>().Value;
    client.BaseAddress = new Uri(o.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

Key (repoya GİRMEZ):
```bash
dotnet user-secrets set "Groq:ApiKey" "gsk_..."
```

**Doğrula:** key yoksa 503, varsa 200:
```bash
curl -i -X POST http://localhost:5080/api/meals/52772/ai-summary
```

## 7. Test = canlı API'ye çıkmadan (fake client)
- `Tests/Integration/Fakes/{FakeMealDbClient, FakeAiService}.cs`.
- `Tests/Integration/CustomWebApplicationFactory.cs` — `ConfigureTestServices` + `RemoveAll` + `AddSingleton(fake)`; ayrıca `CreateHost` içinde `db.Database.Migrate()`.
- `Tests/Integration/MealsEndpointTests.cs`, `FavoritesEndpointTests.cs`.

```bash
cd ..
dotnet test    # repo kökü — TaskFlow.sln
```

## 8. Frontend — paylaşılan altyapı
```bash
cd frontend
npm install react-router-dom
```
- `main.tsx` → `<BrowserRouter>`; `app/App.tsx` → `<NavBar/>` + `<Routes/>`.
- `shared/lib/apiClient.ts` → `apiPost/apiDelete` ekle.

## 9. Frontend — meals feature (Tasks aynası)
- `features/meals/{types.ts, mealKeys.ts, api/mealsClient.ts, hooks/*, components/*}`.
- `pages/{MealsPage, MealDetailPage, FavoritesPage}.tsx`.

```bash
npm run test
npm run build
npm run dev     # tarayıcıda /meals
```

## 10. PR
```bash
git switch -c gold/meals-module
git add -A && git commit -m "gold: meals modülü"
git push -u origin gold/meals-module
gh pr create --fill
```
PR şablonundaki tüm kutuları işaretle. CI yeşil olmadan merge yok.

---

## Neden böyle? (senior kararları)
- **Read-model vs entity:** sahip olmadığın veriyi entity yapma; sadece favori (owned) DB'ye gider.
- **Typed HttpClient + arayüz:** testte `RemoveAll` + fake ile canlı ağa çıkmadan doğrularsın.
- **Cache endpoint'te değil service'te:** iş kuralı tek yerde.
- **AI key server-side:** frontend key'i asla görmez; key yoksa graceful 503.
- **Hata kodları:** 404 (yok) / 409 (çakışma) / 503 (AI kapalı) / 502 (AI hata) — hepsi anlamlı, 500 değil.
