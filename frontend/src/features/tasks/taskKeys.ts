/**
 * React Query cache anahtarları tek yerde. Mutation'lar başarılı olunca
 * bu anahtarları invalidate ederek listeyi tazeleriz (Gün 5: CreateTaskForm).
 */
export const taskKeys = {
  all: ['tasks'] as const,
  lists: () => [...taskKeys.all, 'list'] as const,
}
