# Backend — TaskFlow API

ASP.NET Core Web API + EF Core + PostgreSQL.

Bu backend, “AI kod yazdı” demosu değildir. Her endpointte HTTP → validation → application service → EF Core → PostgreSQL akışını görünür tutar.

## Başlangıç

### Gereksinimler

- .NET SDK (repo içindeki `global.json` sürümü)
- Docker Desktop
- PostgreSQL yalnızca Docker üzerinden çalışır

### Çalıştırma

```bash
# Repo kökünde
docker compose up -d postgres

cd backend
dotnet restore

# EF Core CLI aracı (makinede bir kez kurulur)
dotnet tool install --global dotnet-ef

dotnet ef database update   # Tasks tablosunu oluşturur
dotnet run                  # http://localhost:5080
```

> Makinende 5432 portu doluysa, repo kökünde `POSTGRES_PORT=5442 docker compose up -d postgres`
> çalıştır ve `appsettings.json` içindeki `Port=5432` değerini eşleştir.

API varsayılan olarak `http://localhost:5080` veya terminalde yazan local URL'de çalışır.

Health kontrolü:

```bash
curl http://localhost:5080/health
```

## Mimari

```text
backend/
├─ Api/
│  ├─ Endpoints/              # HTTP route'ları
│  ├─ Contracts/              # Request/response modelleri
│  └─ Middleware/             # Global hata davranışı
├─ Application/
│  └─ Tasks/                  # Task use-case'leri
├─ Domain/
│  └─ Tasks/                  # Entity ve iş kuralları
├─ Infrastructure/
│  └─ Persistence/            # DbContext, migrations
└─ Tests/
   └─ Integration/            # API davranış testleri
```

## Sorumluluk sınırları

- **Endpoint:** HTTP ile ilgilenir; iş kuralı barındırmaz.
- **Application service:** use-case'i yürütür.
- **Domain entity:** alan kurallarını korur.
- **DbContext:** veri erişimini sağlar.
- **Migration:** şema değişikliğinin versiyonlu kaydıdır.

## İlk endpointler

| Method | Route | Görev |
|---|---|---|
| GET | `/health` | Uygulama ayakta mı? |
| GET | `/api/tasks` | Task listesini getir |
| POST | `/api/tasks` | Yeni task oluştur |
| PATCH | `/api/tasks/{id}/status` | Task durumunu değiştir |
| DELETE | `/api/tasks/{id}` | Task sil |

### Meals gold modülü + Favorites (referans)

| Method | Route | Görev |
|---|---|---|
| GET | `/api/meals?search=&category=` | Tarif listesi (dış API + cache) |
| GET | `/api/meals/categories` | Kategori listesi (filtre kaynağı) |
| GET | `/api/meals/{id}` | Tarif detayı |
| POST | `/api/meals/{id}/ai-summary` | AI özet/etiket (Groq) |
| GET/POST/DELETE | `/api/favorites` | Favori CRUD (owned, EF) |

Desen: dış API → **typed `HttpClient`** (`IMealDbClient`) → **`IMemoryCache`** → application service → contract.
Dış veri Domain entity DEĞİL (read-model); sadece favori owned entity + migration.
Komut-komut kurulum: [`../docs/reference-meals.md`](../docs/reference-meals.md).

### AI (Groq) key
```bash
dotnet user-secrets set "Groq:ApiKey" "gsk_..."   # repoya girmez
```
Key yoksa AI endpoint'i `503` döner (crash yok). Testler `FakeAiService` kullanır → CI'da key gerekmez.

## AI reference → student mirror modeli

### Reference: AI üretir

`CreateTask` modülü:

```text
POST /api/tasks
CreateTaskRequest
CreateTaskService
TaskResponse
Validation
Integration test
```

Stajyer bu modülü anlayıp şu soruları cevaplar:

- Endpoint neden DbContext çağırmıyor?
- Validation neden frontend'e bırakılmadı?
- Başarılı create neden `201 Created`?
- Hangi test neyi garanti ediyor?

### Mirror: stajyer yazar

`UpdateTaskStatus` modülü:

```text
PATCH /api/tasks/{id}/status
UpdateTaskStatusRequest
UpdateTaskStatusService
Not found davranışı
Validation
Integration test
```

Bu modülde AI yalnızca plan, code review ve hata analizi için kullanılır. Baştan dosya üretmesi yasaktır.

## Test

```bash
# Sadece backend integration testleri
cd backend
dotnet test Tests/Integration

# Veya repo kökünden tüm çözümü (TaskFlow.sln) test et
cd ..
dotnet test
```

Minimum davranış testleri:

- Başarılı task oluşturma → `201`
- Boş/geçersiz başlık → `400`
- Olmayan task statü güncelleme → `404`
- Task listesi kalıcı veriyi döner → `200`

## Migration

```bash
cd backend

# Yeni migration
dotnet ef migrations add AddTaskStatus

# Uygula
dotnet ef database update
```

Migration dosyalarını elle silmek veya düzenlemek son çaredir. Önce neden oluştuğunu anlayın.

## Hata davranışı

- `400`: kullanıcı isteği geçersiz
- `404`: kaynak yok
- `409`: iş kuralı çakışması
- `500`: beklenmeyen sunucu hatası

Client'a stack trace dönülmez. Ayrıntı logda kalır.

## AI prompt: backend task planı

```text
Sen kıdemli bir ASP.NET Core backend partnerisin.

Task:
[ÖRNEK: Task status güncelleme]

Mevcut mimari:
- Endpoint -> Application Service -> EF Core DbContext -> PostgreSQL
- Feature-first klasör yapısı
- Minimal API veya controller yaklaşımını mevcut koda uyumlu seç
- Global hata middleware mevcut

Önce kod yazma.
Şunları üret:
1. API contract
2. Validation kuralları
3. HTTP status davranışları
4. Dosya değişiklik listesi
5. Edge-case listesi
6. Integration test senaryoları
7. En basit uygulama sırası
```

## Pull Request öncesi kontrol

```text
[ ] dotnet build
[ ] dotnet test
[ ] Endpoint Swagger/OpenAPI veya HTTP istemcisi ile denendi
[ ] Hatalı input denendi
[ ] .env / secret commit edilmedi
[ ] Migration oluştuysa repoya eklendi
[ ] AI katkısı PR açıklamasında yazıldı
[ ] Stajyer endpoint akışını anlatabiliyor
```
