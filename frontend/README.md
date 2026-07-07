# Frontend — TaskFlow Web

React + TypeScript + Vite.

Bu frontend, sadece ekran çizmek için değil; kullanıcı aksiyonlarını, loading/error/empty durumlarını ve API ile veri akışını kontrollü yönetmek için tasarlanır.

## Başlangıç

### Gereksinimler

- Node.js (repo içindeki `.nvmrc` veya `package.json` engines sürümü)
- npm
- Backend API'nin çalışıyor olması

### Environment

`.env.example` dosyasını `.env` olarak kopyalayın:

```bash
VITE_API_BASE_URL=http://localhost:5080
```

### Çalıştırma

```bash
cd frontend
npm install
npm run dev
```

Tarayıcıda terminalin verdiği local URL'yi açın.

## Klasör yapısı

```text
frontend/
├─ src/
│  ├─ app/                    # Router, provider, uygulama girişi
│  ├─ pages/                  # Sayfalar
│  ├─ features/
│  │  └─ tasks/
│  │     ├─ api/              # tasksClient
│  │     ├─ components/       # feature componentleri
│  │     ├─ hooks/            # query/mutation hook'ları
│  │     ├─ types.ts
│  │     └─ taskKeys.ts
│  ├─ shared/
│  │  ├─ components/          # generic UI parçaları
│  │  └─ lib/                 # fetch client vb.
│  └─ main.tsx
└─ README.md
```

## State ayrımı

| State türü | Örnek | Nerede yaşar? |
|---|---|---|
| Server state | task listesi | React Query / query hook |
| Form state | başlık inputu | form component |
| UI state | modal açık mı? | page veya yakın parent |
| URL state | filtre parametresi | router/search params |

Task listesini rastgele `useState` ile kopyalamayın. API'nin kaynağı olduğu veriyi server state olarak yönetin.

## AI reference → student mirror modeli

### Reference: AI üretir

`CreateTaskForm`:

```text
- Başlık inputu
- Client-side validation
- useCreateTask mutation
- pending state
- error state
- başarılı submit sonrası liste yenileme
```

Stajyer bu soruları cevaplar:

- Input state neden form içinde?
- Liste verisi neden server state?
- Submit butonu pending iken neden disable?
- API hata mesajı kullanıcıya nasıl gösteriliyor?
- Başarılı create sonrası neden query invalidate ediliyor?

### Mirror: stajyer yazar

Aşağıdakilerden biri:

```text
- UpdateTaskStatusButton
- TaskFilterBar
- DeleteTaskButton
- EmptyTaskState
```

AI bu modül için yalnızca plan, review, hata analizi ve test senaryosu desteği verir. Tam component üretmesi yasaktır.

## API client örneği

Tüm HTTP çağrılarını component içine gömmeyin.

```text
features/tasks/api/tasksClient.ts
```

Bu dosyanın sorumluluğu:

- base URL kullanmak
- HTTP isteği atmak
- başarısız response'u normalize etmek
- DTO döndürmek

Component'in sorumluluğu:

- kullanıcı etkileşimi
- form
- görünüm
- loading/error state'i göstermek

## Test

```bash
cd frontend
npm run test
npm run build
```

Minimum testler:

- Task listesi loading görünümü
- API hata görünümü
- Form submit butonunun pending iken disabled olması
- Empty state

## AI prompt: frontend task planı

```text
Sen kıdemli bir React + TypeScript partnerisin.

Task:
[ÖRNEK: Task durum değiştirme butonu]

Mevcut mimari:
- React + TypeScript + Vite
- feature-first klasör yapısı
- API client feature altında
- Server state query/mutation hook'larıyla yönetiliyor
- UI kit ekleme; mevcut primitive componentleri kullan
- Backend API contract: [EKLE]

Önce kod yazma.
Şunları üret:
1. Component sorumluluğu
2. State ayrımı
3. API çağrısı akışı
4. Hata / loading / success davranışı
5. Dosya listesi
6. Edge-case'ler
7. Test senaryoları
8. En basit implementation planı
```

## Pull Request öncesi kontrol

```text
[ ] npm run build
[ ] npm run test
[ ] Backend kapalıyken hata görünümü kontrol edildi
[ ] Yavaş ağ / pending state kontrol edildi
[ ] Empty state kontrol edildi
[ ] VITE_API_BASE_URL .env'den okunuyor
[ ] API çağrısı component içine dağılmadı
[ ] AI katkısı PR açıklamasında yazıldı
[ ] Stajyer veri akışını çizerek anlatabiliyor
```
