# Brief — Stajyer 2: Kütüphane modülü

**Veri:** Open Library — `https://openlibrary.org/` (anahtarsız). ⚠️ **`User-Agent` header'ı ZORUNLU.** Detay: [`../data-sources.md`](../data-sources.md).
**Aynalanacak gold:** `Meals` modülü. Her rung'un cevabı orada.

## Rung planı (sırayla, her biri 1 issue → 1 PR)
| Issue | Rung | İş |
|---|---|---|
| S2-1 | 1 | `MealsPage`'e A-Z / Z-A sıralama + "N sonuç" sayacı (var olan sayfayı editle) |
| S2-2 | 2 | Meal detayına `HasVideo` + `InstructionWordCount` (service→DTO→UI→test) |
| S2-3 | 3 | **Kütüphane read-only modülü**: `IOpenLibraryClient` (User-Agent!) + `GET /api/books` + `LibraryPage` |
| S2-4 | 4 | Kitap detay + dil/yıl filtresi + "Okuma Listem" (`SavedBook` + `ReadingStatus` enum + migration) |
| S2-5 | 5 | `POST /api/books/{id}/ai-summary` (Groq) + detay AI paneli |

## Sayfa içeriği (rung-3 sonrası)
- `/library`: kitap arama listesi (kapak + başlık + yazar + yıl), loading/error/empty.
- `/library/:id`: detay (açıklama, yıl, dil), "Okuma listeme ekle" + durum (WantToRead/Reading/Read), (rung-5) AI tanıtım/etiket.

## Backend yeni dosyalar (Meals aynası)
`Application/Library/*`, `Infrastructure/External/OpenLibrary/*`, `Api/Endpoints/BooksEndpoints.cs`,
`Api/Contracts/Book*.cs`. `Program.cs`'e SADECE ekle. Typed client'ta default `User-Agent` header'ı ekle.

## DOKUNMA
Meals / Favorites / Tasks modülleri, `Program.cs` mevcut satırları, Stajyer 1'in dosyaları,
`IAiService`/`GroqAiService` iç yapısı (sadece çağır).

## Önemli
- rung-4'te `ReadingStatus` enum'ını `HasConversion<int>()` ile map et (Tasks'taki `TaskItemStatus` aynası).
- İntegration testinde `FakeOpenLibraryClient` kullan.
- `User-Agent` header'ı olmadan Open Library istekleri engellenebilir.
