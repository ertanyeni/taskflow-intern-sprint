# Veri Kaynakları

Tüm dış veri **backend'den** (C# `HttpClient`) çekilir, cache'lenir ve frontend'e sunulur.
Frontend hiçbir zaman dış API'yi veya AI key'ini doğrudan görmez. Sebep:
- CORS derdi olmaz (server-side çağrı),
- API/AI key'i sızmaz (server-side kalır),
- rate limit tek noktadan cache ile yönetilir.

## Veri API'leri (hepsi anahtarsız)

| Modül | API | Base URL | Auth | Rate limit | Not |
|---|---|---|---|---|---|
| Meals (gold) | TheMealDB | `https://www.themealdb.com/api/json/v1/1/` | Yok (test key `1` URL'de) | Bol | list+detail+filter+zengin metin |
| Pokédex (Stajyer 1) | PokeAPI | `https://pokeapi.co/api/v2/` | Yok | Fair-use (aşırıda IP ban) | pagination + ilişkili veri; cache şart |
| Kütüphane (Stajyer 2) | Open Library | `https://openlibrary.org/` | Yok | Yumuşak | **`User-Agent` header'ı ZORUNLU** |

### Örnek istekler (curl)
```bash
# TheMealDB
curl "https://www.themealdb.com/api/json/v1/1/categories.php"
curl "https://www.themealdb.com/api/json/v1/1/search.php?s=chicken"
curl "https://www.themealdb.com/api/json/v1/1/lookup.php?i=52772"

# PokeAPI
curl "https://pokeapi.co/api/v2/pokemon?limit=20"
curl "https://pokeapi.co/api/v2/pokemon/pikachu"

# Open Library (User-Agent şart!)
curl -H "User-Agent: TaskFlowInternSprint/1.0 (ertyeni@gmail.com)" \
  "https://openlibrary.org/search.json?q=dune&limit=10"
```

> Open Library typed client'ında default header:
> `client.DefaultRequestHeaders.UserAgent` → `TaskFlowInternSprint/1.0 (ertyeni@gmail.com)`.
> Header olmadan istekler engellenebilir.

## AI: Groq (ücretsiz, OpenAI-uyumlu)

Endpoint: `POST https://api.groq.com/openai/v1/chat/completions`
Auth: `Authorization: Bearer gsk_...`

### Ücretsiz Groq key alma (her stajyer kendi key'ini alır)
1. https://console.groq.com → ücretsiz kayıt (kredi kartı yok).
2. **API Keys** → **Create API Key** → `gsk_...` kopyala.
3. Backend'de secret olarak sakla (repoya GİRMEZ):
   ```bash
   cd backend
   dotnet user-secrets set "Groq:ApiKey" "gsk_..."
   ```
   Alternatif: `Groq__ApiKey` environment değişkeni.

### Key yoksa ne olur?
Backend crash etmez: AI endpoint'i **503** ("AI yapılandırılmadı") döner, frontend anlamlı bir hata gösterir.
Testler `FakeAiService` kullandığı için **CI'da gerçek key gerekmez.**

## Güvenlik kuralları
- API/AI key'i **asla** commit edilmez (`.gitignore` `.env*` ve `user-secrets` bunu sağlar).
- Key **asla** frontend'e gönderilmez.
- Testler **asla** canlı dış API'ye / Groq'a çıkmaz (fake client'lar).
