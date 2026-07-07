import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { mealsClient } from '../api/mealsClient'
import { favoriteKeys } from '../mealKeys'
import type { CreateFavorite } from '../types'

/** Bir modülün favorileri (server state). */
export function useFavorites(module: string) {
  return useQuery({
    queryKey: favoriteKeys.list(module),
    queryFn: () => mealsClient.getFavorites(module),
  })
}

/** Favori ekle → başarıda ilgili modülün favori listesini invalidate et. */
export function useAddFavorite(module: string) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (body: CreateFavorite) => mealsClient.addFavorite(body),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: favoriteKeys.list(module) })
    },
  })
}

/** Favori sil → başarıda listeyi invalidate et. */
export function useRemoveFavorite(module: string) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: string) => mealsClient.removeFavorite(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: favoriteKeys.list(module) })
    },
  })
}
