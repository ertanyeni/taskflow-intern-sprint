# Müfredat — 2 Stajyer × 5 Basamak

Program felsefesi ve genel kurallar için ana rehber: [`AI_SENIOR_PARTNER_REHBERI.md`](../AI_SENIOR_PARTNER_REHBERI.md).

Bu proje **Modüler Keşif Paneli**. Her modül aynı dikey kesiti aynalar:
`React → ASP.NET Core → (dış API / EF Core) → PostgreSQL`.

- **Gold referans (biz yazdık):** `Meals` modülü (TheMealDB). list + detail + filtre + favori (EF) + AI. Her rung'un cevabı burada.
- **Stajyer 1:** `Pokédex` (PokeAPI). **Stajyer 2:** `Kütüphane` (Open Library).
- İki stajyer **paralel, aynı seviye, farklı veri**. Herkes kendi 5 basamağını **sırayla** çıkar.

## 3 Aşama = 5 Basamak (basitten zora)

| Basamak | Aşama | Ne öğrenir | Mimariye dokunur mu? |
|---|---|---|---|
| rung-1 | Aşama 1 | Var olan sayfada function/method editleme | Hayır (sadece mevcut FE) |
| rung-2 | Aşama 1 | Var olan modüle alan ekleme (service→DTO→UI) | Hafif (mevcut katmanlar) |
| rung-3 | Aşama 2 | Yeni endpoint + yeni sayfa (read-only modül) | Evet (yeni dikey kesit) |
| rung-4 | Aşama 2 | Detay + filtre + kalıcılık (yeni EF entity + migration) | Evet (Domain + EF) |
| rung-5 | Aşama 3 | Yeni AI özelliği (mimariye dokunmadan tekrar kullanım) | Hayır (var olanı çağırır) |

## Rung'lar ve gold referans eşlemesi

### rung-1 (Aşama 1) — var olan bir liste sayfasını editle (farklı sayfalar!)
- **S1-1 (Meals):** `MealsPage`'e debounce'lu **arama** + **"N sonuç"** sayacı. Backend zaten `GET /api/meals?search=` destekliyor.
- **S2-1 (Tasks):** **Görevler** listesine **durum filtresi** + **"N sonuç"** sayacı (client-side). `/tasks` demo görevle seed'li.
- Referans paterni (ikisi de): `features/meals/components/MealFilters.tsx` (kategori filtresi).
- ✅ Farklı sayfalar (Meals ≠ Tasks) → **ortak dosya yok, çakışma yok, paralel çalışırlar.**

### rung-2 (Aşama 1) — türetilmiş alan (katmanlar arası, service→DTO→UI→test)
- **S1-2 (Meals):** `IngredientCount` — `MealService` → `MealDetailResponse` → `MealDetailPage` → `MealsEndpointTests`.
- **S2-2 (Tasks):** `AgeDays` ("kaç gün önce") — `TaskService` → `TaskResponse` → `TaskList` → `TasksEndpointTests`.
- Aynı beceri, **farklı modül** → çakışma yok.

### rung-3 (Aşama 2) — kendi read-only modülün
- **S1-3:** Pokédex. **S2-3:** Kütüphane.
- Gold Meals backend zincirini aynala: `I<Api>Client` (typed HttpClient) → `<Domain>Service` (+cache) → `<X>Endpoints` → Contracts. Frontend: `features/<domain>/*` + yeni sayfa.
- Zorunlu: loading / error / empty durumları + **fake client ile integration testi** (canlı API'ye çıkma).

### rung-4 (Aşama 2) — detay + filtre + kalıcılık
- **S1-4:** Pokémon detay + tip filtresi + "Takımım" (`PokemonFavorite` entity + migration).
- **S2-4:** Kitap detay + dil/yıl filtresi + "Okuma Listem" (`SavedBook` + `ReadingStatus` enum + migration).
- Referans: `Domain/Favorites/FavoriteItem.cs`, `Application/Favorites/*`, `Api/Endpoints/FavoritesEndpoints.cs`, `Migrations/*_AddFavorites.cs`.
- ⚠️ Migration'lar sıralı merge edilmeli.

### rung-5 (Aşama 3) — AI, mimariye dokunmadan
- **S1-5:** Pokémon strateji özeti/etiketi. **S2-5:** Kitap tanıtım/etiket.
- Var olan `IAiService`'i **çağır** (değiştirme). Yeni endpoint `POST /api/<domain>/{id}/ai-summary` + detay sayfasında panel + cache.
- Referans: `MealsEndpoints` AI endpoint'i, `features/meals/components/AiSummaryPanel.tsx`, `hooks/useAiSummary.ts`.

## Her rung için review soruları (mentor en az 3 sorar)
1. Bu veri neden Domain entity değil (Meals/Pokémon) ama favori entity? Sahiplik farkı ne?
2. Dış API'yi neden `IXClient` arayüzü arkasına koyduk? Testte ne kazandırdı?
3. Cache TTL'i neden bu değer? Kısa/uzun olsa ne bozulur?
4. AI key nerede yaşıyor? Frontend'e sızabilir mi? Key yoksa ne oluyor?
5. Bu endpoint neden 404/409/503/502 dönüyor, 500 değil?
6. Aynı isteği kullanıcı iki kez atarsa (double click) ne olur?
7. Bu PR'ı yarıya küçültmenin yolu var mı?

## Teslim paketi (her rung)
GitHub Issue → branch (`feat/<intern>/<rung>-<slug>`) → küçük commit'ler → PR (issue'yu kapatır) → yeşil CI → mentor review → merge. PR'da AI katkısı + test kanıtı zorunlu.

## Değerlendirme rubriği
| Alan | Ağırlık | Başarılı davranış |
|---|---:|---|
| Mimari anlama | %25 | Veri akışını çizerek anlatır; katman sorumluluklarını bilir |
| AI kullanımı | %20 | Plan ister, küçük diff üretir, çıktıyı doğrular |
| Kod üretimi | %20 | Gold paterni aynalayıp kendi modülünü çıkarır |
| Test & debug | %20 | Fake ile test yazar; hata kodundan kök neden bulur |
| GitHub iletişimi | %15 | Küçük PR, net açıklama, kanıt |
