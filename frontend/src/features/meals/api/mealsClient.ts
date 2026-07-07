import { apiDelete, apiGet, apiPost } from '../../../shared/lib/apiClient'
import type { AiSummary, CreateFavorite, Favorite, MealCategory, MealDetail, MealSummary } from '../types'

/**
 * Meals feature'ının API sözleşmesi. Hook'lar/component'ler HTTP detaylarını bilmeden
 * bu fonksiyonları çağırır. Tasks'taki tasksClient.ts ile aynı fikir.
 */
export const mealsClient = {
  getMeals: (search: string, category: string) => {
    const params = new URLSearchParams()
    if (search) params.set('search', search)
    if (category) params.set('category', category)
    const qs = params.toString()
    return apiGet<MealSummary[]>(`/api/meals${qs ? `?${qs}` : ''}`)
  },

  getCategories: () => apiGet<MealCategory[]>('/api/meals/categories'),

  getMeal: (id: string) => apiGet<MealDetail>(`/api/meals/${id}`),

  getAiSummary: (id: string) => apiPost<AiSummary>(`/api/meals/${id}/ai-summary`),

  getFavorites: (module: string) => apiGet<Favorite[]>(`/api/favorites?module=${module}`),

  addFavorite: (body: CreateFavorite) => apiPost<Favorite>('/api/favorites', body),

  removeFavorite: (id: string) => apiDelete(`/api/favorites/${id}`),
}
