import { Link } from 'react-router-dom'
import { useFavorites, useRemoveFavorite } from '../features/meals/hooks/useFavorites'

export function FavoritesPage() {
  const { data: favorites, isPending, isError } = useFavorites('meals')
  const remove = useRemoveFavorite('meals')

  return (
    <main>
      <h1>Favoriler</h1>
      <p>Kaydedilen tarifler (owned veri — PostgreSQL'de EF Core ile saklanır).</p>

      {isPending && <p data-testid="loading">Yükleniyor…</p>}
      {isError && <p role="alert">Favoriler yüklenemedi.</p>}

      {favorites && favorites.length === 0 && (
        <p data-testid="empty-state">Henüz favori yok. Bir tarifin detayında “Favorile”ye bas.</p>
      )}

      {favorites && favorites.length > 0 && (
        <ul>
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
