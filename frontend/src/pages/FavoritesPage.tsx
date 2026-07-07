import { Link } from 'react-router-dom'
import { useFavorites, useRemoveFavorite } from '../features/meals/hooks/useFavorites'

export function FavoritesPage() {
  const { data: favorites, isPending, isError } = useFavorites('meals')
  const remove = useRemoveFavorite('meals')

  return (
    <main>
      <div className="page-head">
        <span className="kicker">FAV · 02 · owned</span>
        <h1>Favoriler</h1>
        <p>Kaydedilen tarifler — bize ait veri, PostgreSQL'de EF Core ile saklanır.</p>
      </div>

      {isPending && <p className="state" data-testid="loading">Yükleniyor…</p>}
      {isError && <p className="state state--error" role="alert">Favoriler yüklenemedi.</p>}

      {favorites && favorites.length === 0 && (
        <p className="state" data-testid="empty-state">
          Henüz favori yok. Bir tarifin detayında “Favorile”ye bas, burada belirsin.
        </p>
      )}

      {favorites && favorites.length > 0 && (
        <ul className="favorites-list">
          {favorites.map((fav) => (
            <li key={fav.id} className="favorite-row">
              <Link to={`/meals/${fav.externalId}`}>{fav.title}</Link>
              <button className="btn" disabled={remove.isPending} onClick={() => remove.mutate(fav.id)}>
                Kaldır
              </button>
            </li>
          ))}
        </ul>
      )}
    </main>
  )
}
