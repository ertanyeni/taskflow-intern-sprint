import type { ReactNode } from 'react'
import { Link } from 'react-router-dom'

const REPO = 'https://github.com/ertanyeni/taskflow-intern-sprint'

interface Step {
  term: string
  gloss: string
  detail: ReactNode
}

const steps: Step[] = [
  {
    term: 'Set up & run',
    gloss: 'kur ve çalıştır',
    detail: (
      <>
        <code>docker compose up -d postgres</code> → <code>dotnet run</code> → <code>npm run dev</code> ·{' '}
        <a href={`${REPO}#hızlı-başlangıç`} target="_blank" rel="noreferrer">README</a>
      </>
    ),
  },
  {
    term: 'Study the gold reference',
    gloss: 'gold örneği incele',
    detail: (
      <>
        <Link to="/meals">Tarifler (Meals)</Link> senin referansın — kodu oku, çalıştır, veri akışını çiz.
      </>
    ),
  },
  {
    term: 'Read the walkthrough',
    gloss: 'komut-komut rehber',
    detail: (
      <>
        Meals'ı adım adım nasıl kurduğumuz —{' '}
        <a href={`${REPO}/blob/main/docs/reference-meals.md`} target="_blank" rel="noreferrer">
          docs/reference-meals.md
        </a>
      </>
    ),
  },
  {
    term: 'Claim your task',
    gloss: 'görevini al',
    detail: (
      <>
        <a href={`${REPO}/issues`} target="_blank" rel="noreferrer">GitHub Issues</a> → Stajyer 1:{' '}
        <code>S1-1</code>, Stajyer 2: <code>S2-1</code> (rung'ları sırayla).
      </>
    ),
  },
  {
    term: 'Branch off',
    gloss: 'branch aç',
    detail: (
      <>
        <code>feat/&lt;intern&gt;/&lt;rung&gt;-&lt;slug&gt;</code> · no direct push to main{' '}
        <em>(main'e direkt push yok)</em>.
      </>
    ),
  },
  {
    term: 'Open a small PR',
    gloss: 'küçük PR aç',
    detail: <>issue'yu kapat → green CI → mentor review → merge.</>,
  },
  {
    term: 'The golden rule',
    gloss: 'altın kural',
    detail: (
      <>
        Anlamadığın kodu merge etme. AI'ı senior partner gibi kullan (plan + review), tek seferde tam dosya
        yazdırma. ·{' '}
        <a href={`${REPO}/blob/main/docs/curriculum.md`} target="_blank" rel="noreferrer">müfredat</a>
      </>
    ),
  },
]

interface Entry {
  call: string
  title: string
  desc: string
  tags: string[]
  to?: string
  stamp: { label: string; kind: 'gold' | 'ref' | 'yours' | 'owned' }
}

const collection: Entry[] = [
  {
    call: 'TSK · 00',
    title: 'Görevler',
    desc: 'İç CRUD iskeleti — bir kaydın uçtan uca PostgreSQL yolculuğu.',
    tags: ['owned', 'EF Core', 'CRUD'],
    to: '/tasks',
    stamp: { label: 'referans', kind: 'ref' },
  },
  {
    call: 'MEA · 01',
    title: 'Tarifler',
    desc: 'Dış API + cache + detay + filtre + favori + AI. Aynalayacağın gold örnek.',
    tags: ['dış API', 'cache', 'EF', 'AI'],
    to: '/meals',
    stamp: { label: 'gold', kind: 'gold' },
  },
  {
    call: 'FAV · 02',
    title: 'Favoriler',
    desc: 'Kaydettiğin dış kayıtlar — bize ait veri, EF Core ile kalıcı.',
    tags: ['owned', 'EF Core'],
    to: '/favorites',
    stamp: { label: 'owned', kind: 'owned' },
  },
  {
    call: 'POK · 03',
    title: 'Pokédex',
    desc: 'Stajyer 1 modülü — PokeAPI üzerinden kendi explorer’ını kur.',
    tags: ['PokeAPI', 'Stajyer 1', 'açık'],
    stamp: { label: 'senin', kind: 'yours' },
  },
  {
    call: 'LIB · 04',
    title: 'Kütüphane',
    desc: 'Stajyer 2 modülü — Open Library üzerinden kitap keşif modülünü kur.',
    tags: ['Open Library', 'Stajyer 2', 'açık'],
    stamp: { label: 'senin', kind: 'yours' },
  },
]

function EntryRow({ entry }: { entry: Entry }) {
  const inner = (
    <>
      <span className="entry__call">{entry.call}</span>
      <span className="entry__main">
        <span className="entry__title">{entry.title}</span>
        <span className="entry__desc">{entry.desc}</span>
        <span className="entry__tags">
          {entry.tags.map((t) => (
            <span key={t}>{t}</span>
          ))}
        </span>
      </span>
      <span className={`stamp stamp--${entry.stamp.kind === 'owned' ? 'ref' : entry.stamp.kind}`}>
        {entry.stamp.label}
      </span>
    </>
  )

  return entry.to ? (
    <Link to={entry.to} className="entry">
      {inner}
    </Link>
  ) : (
    <div className="entry entry--locked">{inner}</div>
  )
}

export function HomePage() {
  return (
    <main>
      <div className="masthead">
        <span className="kicker rise rise-1">AI Senior Partner · Full-Stack Sprint</span>
        <h1 className="masthead__title rise rise-2">Keşif Kütüphanesi</h1>
        <p className="masthead__lede rise rise-3">
          Her modül aynı dikey kesiti aynalar: React → ASP.NET Core → (dış API / EF Core) → PostgreSQL.
          Buradaki koleksiyonu okuyup aynalayarak kendi modülünü kuruyorsun.
        </p>
        <div className="masthead__meta rise rise-4">
          <span><b>5</b> modül</span>
          <span><b>3</b> hazır</span>
          <span><b>2</b> açık</span>
          <span><b>main</b> korumalı</span>
        </div>
      </div>

      <section className="section">
        <div className="section__head">
          <span className="section__num">§1</span>
          <h2 className="section__title">Buradan başla</h2>
          <span className="section__note">ilk gün · sırayla</span>
        </div>
        <ol className="start stagger">
          {steps.map((step, i) => (
            <li className="start__item" key={step.term}>
              <span className="start__index">{String(i + 1).padStart(2, '0')}</span>
              <span className="start__body">
                <span>
                  <span className="start__term">{step.term}</span>{' '}
                  <span className="start__gloss">({step.gloss})</span>
                </span>
                <span className="start__detail">{step.detail}</span>
              </span>
            </li>
          ))}
        </ol>
      </section>

      <section className="section">
        <div className="section__head">
          <span className="section__num">§2</span>
          <h2 className="section__title">Koleksiyon</h2>
          <span className="section__note">gold örneği aynala → kendi modülünü kur</span>
        </div>
        <div className="catalog stagger">
          {collection.map((entry) => (
            <EntryRow key={entry.call} entry={entry} />
          ))}
        </div>
      </section>
    </main>
  )
}
