// React Query cache anahtarları. Mutation'lar başarılı olunca ilgili anahtar invalidate edilir.

export const mealKeys = {
  all: ['meals'] as const,
  list: (search: string, category: string) => [...mealKeys.all, 'list', { search, category }] as const,
  categories: () => [...mealKeys.all, 'categories'] as const,
  detail: (id: string) => [...mealKeys.all, 'detail', id] as const,
}

export const favoriteKeys = {
  all: ['favorites'] as const,
  list: (module: string) => [...favoriteKeys.all, module] as const,
}
