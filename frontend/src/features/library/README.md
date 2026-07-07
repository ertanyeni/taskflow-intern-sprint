# features/library/ — Stajyer 2 modülü (AÇIK)

Bu klasör bilerek boş. Stajyer 2 burayı **S2-3 (rung-3)** görevinde doldurur.

Aynalanacak gold referans: `frontend/src/features/meals/`

Veri kaynağı: Open Library — `https://openlibrary.org/search.json?q=...` (anahtarsız).
⚠️ Backend istemcisinde `User-Agent` header'ı ZORUNLU (bkz. `docs/data-sources.md`).

Beklenen dosyalar (rung ilerledikçe):
- `types.ts`, `libraryKeys.ts`, `api/libraryClient.ts`
- `hooks/useBooks.ts`, `hooks/useBook.ts`, `hooks/useAiSummary.ts`
- `components/BookList.tsx` (+ test), `BookCard.tsx`, `AiSummaryPanel.tsx`

Detay: `docs/briefs/library.md`.
