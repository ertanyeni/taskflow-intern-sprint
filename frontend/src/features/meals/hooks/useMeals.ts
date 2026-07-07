import { useQuery } from '@tanstack/react-query'
import { mealsClient } from '../api/mealsClient'
import { mealKeys } from '../mealKeys'

/** Meal listesi (arama + kategori) server state'i. */
export function useMeals(search: string, category: string) {
  return useQuery({
    queryKey: mealKeys.list(search, category),
    queryFn: () => mealsClient.getMeals(search, category),
  })
}
