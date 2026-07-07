# Mimari

## Genel bakış
Modüler monolith, feature-first. Tek repo, iki uygulama (backend + frontend), tek PostgreSQL.

```text
Browser (React + TS)
  ↓ HTTP / JSON
ASP.NET Core Web API (Minimal API endpoints)
  ↓
Application Service (use-case, cache, orkestrasyon)
  ↓                      ↓
EF Core + PostgreSQL     Typed HttpClient → dış API (TheMealDB / PokeAPI / Open Library / Groq)
```

## İki tür veri
| Tür | Örnek | Nerede yaşar | Domain entity? |
|---|---|---|---|
| **Owned (bize ait)** | Task, Favori | PostgreSQL (EF Core) | **Evet** + migration |
| **Dış (read-model)** | Meal, Pokémon, Kitap | Dış API + cache | **Hayır** |

Bu ayrım tüm modüllerin belkemiği. "Sahip olmadığın veriyi entity yapma."

## Backend katmanları
```text
Api/Endpoints        HTTP route'ları — iş kuralı yok
Api/Contracts        dış sözleşme (request/response record)
Api/Middleware       global hata (ExceptionHandlingMiddleware)
Application/<Domain> use-case + cache + orkestrasyon (IXService)
Infrastructure/External/<Api>  typed HttpClient (IXClient)
Infrastructure/Persistence     AppDbContext + migrations
Domain/<Owned>       owned entity'ler
```

## Frontend katmanları (feature-first)
```text
app/                 router, NavBar, uygulama girişi
pages/               sayfa akışları
features/<domain>/   api (client) + hooks (query/mutation) + components + types + keys
shared/lib/          apiClient (tek HTTP kapısı)
```

## Dış çağrı neden hep backend'den?
- **CORS** derdi olmaz (server-side).
- **Key sızmaz** (Groq key server-side).
- **Rate limit** tek noktadan cache ile yönetilir.

## Test stratejisi
Dış bağımlılık (`IMealDbClient`, `IAiService`) arayüz arkasında. Integration testinde
`ConfigureTestServices` + `RemoveAll` + fake ile değiştirilir → testler canlı ağa çıkmaz,
CI dış key/ağ gerektirmez.

## Modüller
| Modül | Tür | Sahip |
|---|---|---|
| Tasks | owned CRUD | çalışan iskelet |
| Meals | dış + owned favori + AI | **gold referans** |
| Favorites | owned | gold |
| Pokédex | dış + owned + AI | Stajyer 1 |
| Kütüphane | dış + owned + AI | Stajyer 2 |
