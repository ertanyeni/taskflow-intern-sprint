import { useQuery } from '@tanstack/react-query'
import { tasksClient } from '../api/tasksClient'
import { taskKeys } from '../taskKeys'

/**
 * Task listesi server state'i. Kaynağı API'dir; useState ile kopyalanmaz.
 */
export function useTasks() {
  return useQuery({
    queryKey: taskKeys.lists(),
    queryFn: tasksClient.getTasks,
  })
}
