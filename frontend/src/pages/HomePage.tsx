import { Link } from 'react-router-dom'

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
