import { NavLink } from 'react-router-dom'

const links = [
  { to: '/', label: 'Koleksiyon', end: true },
  { to: '/tasks', label: 'Görevler' },
  { to: '/meals', label: 'Tarifler' },
  { to: '/favorites', label: 'Favoriler' },
]

export function NavBar() {
  return (
    <header className="topbar">
      <div className="topbar__brand">
        <span className="topbar__brand-name">Keşif Kütüphanesi</span>
        <span className="topbar__brand-sub">Koleksiyon № 01</span>
      </div>
      <nav>
        <ul className="topbar__nav">
          {links.map((link) => (
            <li key={link.to}>
              <NavLink
                to={link.to}
                end={link.end}
                className={({ isActive }) => (isActive ? 'topbar__link topbar__link--active' : 'topbar__link')}
              >
                {link.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
    </header>
  )
}
