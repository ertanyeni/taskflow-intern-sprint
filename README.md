# AI Senior Partner ile Full-Stack Stajyer Sprinti

Bu repo, kod yazmayı yeni öğrenen mühendislik öğrencilerinin AI'ı “kodu benim yerime yaz” aracı olarak değil, **senior partner** olarak kullanmasını öğretir.

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

## Repo düzeni

```text
/
├─ backend/                  # ASP.NET Core Web API
├─ frontend/                 # React + TypeScript
├─ docker-compose.yml        # PostgreSQL + local servisler
├─ docs/
│  ├─ architecture.md
│  ├─ api-contract.md
│  └─ ai-decisions.md
└─ README.md
```

## Non-negotiable kurallar

- `main` branch'ine doğrudan push yok.
- Her iş bir GitHub Issue ile başlar.
- Her Issue için ayrı branch açılır.
- AI'ın ürettiği kod, test edilmeden merge edilmez.
- Stajyer anlamadığı kodu merge edemez.
- Secret, token, gerçek müşteri verisi ve gerçek production logu AI'a gönderilmez.
- “Localimde çalışıyor” teslim kriteri değildir.
