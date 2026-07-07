import { NavLink } from 'react-router-dom'

const links = [
  { to: '/', label: 'Panel', end: true },
  { to: '/tasks', label: 'Görevler' },
  { to: '/meals', label: 'Tarifler' },
  { to: '/favorites', label: 'Favoriler' },
]

export function NavBar() {
  return (
    <nav className="navbar">
      <span className="navbar__brand">Keşif Paneli</span>
      <ul className="navbar__links">
        {links.map((link) => (
          <li key={link.to}>
            <NavLink
              to={link.to}
              end={link.end}
              className={({ isActive }) => (isActive ? 'navlink navlink--active' : 'navlink')}
            >
              {link.label}
            </NavLink>
          </li>
        ))}
      </ul>
    </nav>
  )
}
