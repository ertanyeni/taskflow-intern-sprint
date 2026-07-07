import { useQuery } from '@tanstack/react-query'
import { mealsClient } from '../api/mealsClient'
import { mealKeys } from '../mealKeys'

/** Kategori listesi (filtre kaynağı). Uzun süre stabil → staleTime yüksek. */
export function useMealCategories() {
  return useQuery({
    queryKey: mealKeys.categories(),
    queryFn: mealsClient.getCategories,
    staleTime: 1000 * 60 * 60, // 1 saat
  })
}
