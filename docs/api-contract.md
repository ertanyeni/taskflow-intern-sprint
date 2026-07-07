# API Sözleşmesi

Base URL (local): `http://localhost:5080`. Tüm gövdeler JSON.

## Health
| Method | Route | Yanıt |
|---|---|---|
| GET | `/health` | `200 { "status": "ok" }` |

## Tasks (owned CRUD iskeleti)
| Method | Route | Açıklama |
|---|---|---|
| GET | `/api/tasks` | Task listesi (`200`) |
| POST/PATCH/DELETE | — | Stajyer öğrenme modülleri (Gün 2+) |

## Meals (gold referans)
| Method | Route | Yanıt |
|---|---|---|
| GET | `/api/meals?search=&category=` | `MealSummaryResponse[]` |
| GET | `/api/meals/categories` | `MealCategoryResponse[]` |
| GET | `/api/meals/{id}` | `MealDetailResponse` / `404` |
| POST | `/api/meals/{id}/ai-summary` | `AiSummaryResponse` / `404` / `503` (key yok) / `502` (AI hata) |

```jsonc
// MealSummaryResponse
{ "id": "52772", "name": "Teriyaki...", "category": "Chicken", "area": "Japanese", "thumbnail": "https://..." }

// MealDetailResponse
{ "id": "...", "name": "...", "category": "...", "area": "...", "instructions": "...",
  "thumbnail": "...", "youtube": "https://... | null", "ingredients": ["2 cups Soy Sauce"], "tags": ["Meat"] }

// AiSummaryResponse
{ "summary": "...", "tags": ["..."], "model": "llama-3.1-8b-instant", "cached": false }
```

## Favorites (owned, EF)
| Method | Route | Yanıt |
|---|---|---|
| GET | `/api/favorites?module=meals` | `FavoriteResponse[]` |
| POST | `/api/favorites` | `201 FavoriteResponse` / `400` / `409` (duplicate) |
| DELETE | `/api/favorites/{id}` | `204` / `404` |

```jsonc
// CreateFavoriteRequest
{ "module": "meals", "externalId": "52772", "title": "Teriyaki", "thumbnail": "https://... | null" }
```

## HTTP kodları
`200` ok · `201` oluşturuldu · `204` içerik yok · `400` geçersiz istek · `404` yok
· `409` iş kuralı çakışması · `502` dış AI hatası · `503` AI yapılandırılmadı · `500` beklenmeyen.
Client'a stack trace dönülmez (global middleware).

## Stajyer modülleri (aynı desen, açık)
| Modül | Route deseni |
|---|---|
| Pokédex | `/api/pokemon`, `/api/pokemon/{id}`, `/api/pokedex/favorites`, `/api/pokemon/{id}/ai-summary` |
| Kütüphane | `/api/books`, `/api/books/{id}`, `/api/library/reading-list`, `/api/books/{id}/ai-summary` |
