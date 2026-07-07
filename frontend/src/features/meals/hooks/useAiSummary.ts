import { useMutation } from '@tanstack/react-query'
import { mealsClient } from '../api/mealsClient'

/**
 * AI özeti bir MUTATION'dır (POST, yan etkili/maliyetli iş tetikler), query değil.
 * Kullanıcı butona basınca çalışır; otomatik değil.
 */
export function useAiSummary(mealId: string | undefined) {
  return useMutation({
    mutationFn: () => mealsClient.getAiSummary(mealId!),
  })
}
