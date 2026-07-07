import { useAddFavorite, useFavorites, useRemoveFavorite } from '../hooks/useFavorites'

interface FavoriteButtonProps {
  module: string
  externalId: string
  title: string
  thumbnail: string | null
}

/**
 * Kendi kendine yeten favori butonu: favori listesinden bu kaydın durumunu okur,
 * ekleme/silme mutation'larını yönetir. Başarıda liste otomatik invalidate olur (hook içinde).
 */
export function FavoriteButton({ module, externalId, title, thumbnail }: FavoriteButtonProps) {
  const { data: favorites } = useFavorites(module)
  const add = useAddFavorite(module)
  const remove = useRemoveFavorite(module)

  const existing = favorites?.find((f) => f.externalId === externalId)
  const pending = add.isPending || remove.isPending

  if (existing) {
    return (
      <button className="btn btn--fav-active" disabled={pending} onClick={() => remove.mutate(existing.id)}>
        ★ Favorilerimde
      </button>
    )
  }

  return (
    <button
      className="btn"
      disabled={pending}
      onClick={() => add.mutate({ module, externalId, title, thumbnail })}
    >
      ☆ Favorile
    </button>
  )
}
