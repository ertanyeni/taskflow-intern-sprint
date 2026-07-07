# AI Karar Günlüğü

Bu programda AI, kodu yazan değil **senior partner**dır. Buraya mimari/teknik kararlar,
alternatifler ve AI'ın önerip **uygulanmayan** fikirleri yazılır. Her PR bir satır ekler.

## Format
```
### [tarih] Karar başlığı
- Bağlam:
- Seçenekler:
- Karar + gerekçe:
- AI'ın önerip almadığımız fikir:
```

---

### [2026-07] Uygulama modüler "Keşif Paneli"ne dönüştü
- Bağlam: Stajyerlerin "benzer ama farklı sayfalar" + free data + AI ile pratik yapması gerekiyordu.
- Seçenekler: (a) TaskFlow'u tek domain büyüt, (b) modüler panel + explorer modülleri, (c) sıfırdan yeni app.
- Karar: (b). TaskFlow iç-CRUD referansı kalır; her modül aynı dikey kesiti aynalar.
- Alınmayan: Mikroservis/ayrı repo — öğrenme için gereksiz karmaşa.

### [2026-07] Dış API için AI seçimi = Groq (Gemini değil)
- Bağlam: Ücretsiz text LLM lazım.
- Seçenekler: Gemini (anahtar hazır), Groq (OpenAI-uyumlu), OpenRouter, HF.
- Karar: **Groq** — OpenAI-uyumlu `chat/completions` endüstri standardı desenini öğretir;
  aynı soyutlama ileride başka sağlayıcıya kolay taşınır.
- Alınmayan: Gemini'nin kendine özgü gövde formatı (öğretim değeri daha düşük).

### [2026-07] Dış veri Domain entity DEĞİL
- Karar: Meals/Pokémon/Kitap = read-model + cache; sadece favori/task owned entity + migration.
- Gerekçe: sahiplik sınırı; gereksiz migration ve senkron derdi olmaz.

### [2026-07] Dış çağrılar arayüz arkasında (IXClient)
- Karar: `IMealDbClient`/`IAiService` soyutlaması; testte fake ile değiştir.
- Gerekçe: canlı ağa çıkmadan hızlı/kararlı integration testi; CI dış key gerektirmez.
