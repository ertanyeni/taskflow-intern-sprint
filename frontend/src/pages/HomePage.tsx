import { Link } from 'react-router-dom'

const REPO = 'https://github.com/ertanyeni/taskflow-intern-sprint'

const modules = [
  { to: '/tasks', title: 'Görevler (TaskFlow)', desc: 'İç CRUD referansı — PostgreSQL kalıcılığı.', badge: 'referans' },
  { to: '/meals', title: 'Tarifler (Meals)', desc: 'Dış API + cache + detay + filtre + favori + AI. Gold örnek.', badge: 'gold' },
  { to: '/favorites', title: 'Favoriler', desc: 'Kaydettiğin dış kayıtlar (owned veri, EF).', badge: '' },
]

export function HomePage() {
  return (
    <main>
      <h1>Modüler Keşif Paneli</h1>
      <p>
        Her modül aynı mimariyi aynalar: React → ASP.NET Core → (dış API / EF Core) → PostgreSQL.
        Stajyerler bu paterni aynalayarak kendi modüllerini kurar.
      </p>

      {/* İlk açılışta stajyeri karşılayan, satır satır yönlendirme */}
      <section className="onboarding">
        <h2>Buradan başla 👇</h2>
        <ol className="steps">
          <li>
            <span className="steps__n">1</span>
            <span><b>Kur & çalıştır:</b> <code>docker compose up -d postgres</code> → backend <code>dotnet run</code> → frontend <code>npm run dev</code> · <a href={`${REPO}#hızlı-başlangıç`} target="_blank" rel="noreferrer">README</a></span>
          </li>
          <li>
            <span className="steps__n">2</span>
            <span><b>Gold örneği incele:</b> <Link to="/meals">Tarifler (Meals)</Link> senin referansın — kodu oku, çalıştır, veri akışını çiz.</span>
          </li>
          <li>
            <span className="steps__n">3</span>
            <span><b>Komut-komut rehber:</b> Meals'ı nasıl kurduğumuz — <a href={`${REPO}/blob/main/docs/reference-meals.md`} target="_blank" rel="noreferrer">docs/reference-meals.md</a></span>
          </li>
          <li>
            <span className="steps__n">4</span>
            <span><b>Görevini al:</b> <a href={`${REPO}/issues`} target="_blank" rel="noreferrer">GitHub Issues</a> → Stajyer 1: <code>S1-1</code>, Stajyer 2: <code>S2-1</code> (rung'ları sırayla).</span>
          </li>
          <li>
            <span className="steps__n">5</span>
            <span><b>Branch aç:</b> <code>feat/&lt;stajyer&gt;/&lt;rung&gt;-&lt;slug&gt;</code> · <b>main'e direkt push YOK.</b></span>
          </li>
          <li>
            <span className="steps__n">6</span>
            <span><b>Küçük PR:</b> issue'yu kapat → CI yeşil → mentor review → merge.</span>
          </li>
          <li>
            <span className="steps__n">7</span>
            <span><b>Altın kural:</b> Anlamadığın kodu merge etme; AI'ı senior partner gibi kullan (plan + review), tek seferde tam dosya yazdırma. · <a href={`${REPO}/blob/main/docs/curriculum.md`} target="_blank" rel="noreferrer">müfredat</a></span>
          </li>
        </ol>
      </section>

      <h2>Modüller</h2>
      <div className="module-grid">
        {modules.map((m) => (
          <Link key={m.to} to={m.to} className="module-card">
            <div className="module-card__head">
              <strong>{m.title}</strong>
              {m.badge && <span className={`badge badge--${m.badge}`}>{m.badge}</span>}
            </div>
            <p>{m.desc}</p>
          </Link>
        ))}

        <div className="module-card module-card--todo">
          <div className="module-card__head">
            <strong>Pokédex / Kütüphane</strong>
            <span className="badge badge--todo">stajyer</span>
          </div>
          <p>Stajyer 1 (PokeAPI) ve Stajyer 2 (Open Library) modülleri buraya gelecek.</p>
        </div>
      </div>
    </main>
  )
}
