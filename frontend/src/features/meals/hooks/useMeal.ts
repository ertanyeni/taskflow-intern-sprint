import { useQuery } from '@tanstack/react-query'
import { mealsClient } from '../api/mealsClient'
import { mealKeys } from '../mealKeys'

/** Tek meal detayı. id boşsa sorgu çalışmaz (enabled). */
export function useMeal(id: string | undefined) {
  return useQuery({
    queryKey: mealKeys.detail(id ?? ''),
    queryFn: () => mealsClient.getMeal(id!),
    enabled: Boolean(id),
  })
}
