# Brief — Stajyer 1: Pokédex modülü

**Veri:** PokeAPI — `https://pokeapi.co/api/v2/` (anahtarsız, fair-use). Detay: [`../data-sources.md`](../data-sources.md).
**Aynalanacak gold:** `Meals` modülü. Her rung'un cevabı orada.

## Rung planı (sırayla, her biri 1 issue → 1 PR)
| Issue | Rung | İş |
|---|---|---|
| S1-1 | 1 | `MealsPage`'e debounce'lu arama + "N sonuç" sayacı (var olan sayfayı editle) |
| S1-2 | 2 | Meal detayına `IngredientCount` (service→DTO→UI→test) |
| S1-3 | 3 | **Pokédex read-only modülü**: `IPokeApiClient` + `GET /api/pokemon` + `PokedexPage` |
| S1-4 | 4 | Pokémon detay + tip filtresi + "Takımım" (`PokemonFavorite` entity + migration) |
| S1-5 | 5 | `POST /api/pokemon/{id}/ai-summary` (Groq) + detay AI paneli |

## Sayfa içeriği (rung-3 sonrası)
- `/pokedex`: pagination'lı liste (isim + görsel + tip), loading/error/empty.
- `/pokedex/:id`: detay (tipler, yetenekler, stats), "Takımıma ekle", (rung-5) AI strateji özeti.

## Backend yeni dosyalar (Meals aynası)
`Application/Pokedex/*`, `Infrastructure/External/PokeApi/*`, `Api/Endpoints/PokemonEndpoints.cs`,
`Api/Contracts/Pokemon*.cs`. `Program.cs`'e SADECE ekle.

## DOKUNMA
Meals / Favorites / Tasks modülleri, `Program.cs` mevcut satırları, Stajyer 2'nin dosyaları,
`IAiService`/`GroqAiService` iç yapısı (sadece çağır).

## Önemli
- rung-3 read-only (DB yok). rung-4'te kendi favori entity'ni kur.
- İntegration testinde `FakePokeApiClient` kullan (canlı API'ye çıkma).
- Cache şart (PokeAPI fair-use).
