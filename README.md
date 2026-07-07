# AI Senior Partner ile Full-Stack Stajyer Sprinti

Bu repo, kod yazmayı yeni öğrenen mühendislik öğrencilerinin AI'ı “kodu benim yerime yaz” aracı olarak değil, **senior partner** olarak kullanmasını öğretir.

Uygulama bir **Modüler Keşif Paneli**dir: her modül aynı dikey kesiti aynalar
(`React → ASP.NET Core → (dış API / EF Core) → PostgreSQL`). Bir **gold referans modül**
(Meals) uçtan uca yazılıdır; stajyerler bunu aynalayarak kendi modüllerini kurar.

## Stack

- Backend: ASP.NET Core Web API + C#
- Frontend: React + TypeScript + Vite
- Veri: PostgreSQL
- Local ortam: Docker Compose
- Ekip akışı: GitHub Issues + Branch + Pull Request

## Başarı tanımı

Stajyerin yalnızca çalışan uygulama göstermesi yeterli değildir. Şunları açıklayabilmelidir:

1. Kullanıcı aksiyonunun frontend'den API'ye, servise ve PostgreSQL'e nasıl ulaştığı.
2. AI'ın kurduğu mimaride her klasörün ve her katmanın sorumluluğu.
3. AI'ın yazdığı referans modül ile kendi yazdığı benzer modül arasındaki farklar.
4. Bir test veya build hatasında kök nedenin nasıl bulunduğu.
5. PR'daki değişikliğin riski, test yöntemi ve AI katkısı.

## Modüller

| Modül | Tür | Sahip | Durum |
|---|---|---|---|
| Tasks | owned CRUD | çalışan iskelet | hazır |
| **Meals** | dış API + favori (EF) + AI | **gold referans** | hazır |
| Favorites | owned (EF) | gold | hazır |
| Pokédex | dış API + owned + AI | Stajyer 1 | açık (issue'lar) |
| Kütüphane | dış API + owned + AI | Stajyer 2 | açık (issue'lar) |

## Hızlı başlangıç
```bash
# 1) DB
docker compose up -d postgres            # 5432 doluysa: POSTGRES_PORT=5442 docker compose up -d postgres
# 2) Backend
cd backend && dotnet restore
dotnet tool install --global dotnet-ef   # bir kez
dotnet ef database update && dotnet run   # http://localhost:5080  → /meals, /favorites, Swagger
# 3) Frontend
cd frontend && cp .env.example .env && npm install && npm run dev
# Testler
dotnet test        # repo kökü (backend, fake'lerle)
cd frontend && npm run test && npm run build
```
AI (opsiyonel, canlı özet için): ücretsiz Groq key al → `cd backend && dotnet user-secrets set "Groq:ApiKey" "gsk_..."` (bkz. [docs/data-sources.md](docs/data-sources.md)).

## Repo düzeni

```text
/
├─ backend/                  # ASP.NET Core Web API (Api/Application/Domain/Infrastructure + Tests)
├─ frontend/                 # React + TypeScript (features/{tasks,meals} + pages)
├─ docs/
│  ├─ curriculum.md          # 2 stajyer × 5 basamak merdiveni
│  ├─ reference-meals.md      # gold modülün komut-komut walkthrough'u
│  ├─ data-sources.md         # ücretsiz API'ler + Groq key alma
│  ├─ architecture.md · api-contract.md · ai-decisions.md
│  └─ briefs/{pokedex,library}.md
├─ .github/                  # CI + PR/issue şablonları
├─ docker-compose.yml        # PostgreSQL
├─ TaskFlow.sln
└─ README.md
```

## Nereden başlamalı?
1. [docs/curriculum.md](docs/curriculum.md) — merdiven ve kurallar.
2. [docs/reference-meals.md](docs/reference-meals.md) — gold modülü adım adım incele/çalıştır.
3. GitHub Issues — kendi rung'unu (S1-1 / S2-1) al, `feat/<intern>/<rung>-<slug>` branch'i aç.

## Non-negotiable kurallar

- `main` branch'ine doğrudan push yok.
- Her iş bir GitHub Issue ile başlar.
- Her Issue için ayrı branch açılır.
- AI'ın ürettiği kod, test edilmeden merge edilmez.
- Stajyer anlamadığı kodu merge edemez.
- Secret, token, gerçek müşteri verisi ve gerçek production logu AI'a gönderilmez.
- “Localimde çalışıyor” teslim kriteri değildir.
