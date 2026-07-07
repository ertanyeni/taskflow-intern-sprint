import { apiGet } from '../../../shared/lib/apiClient'
import type { Task } from '../types'

/**
 * Task feature'ının API sözleşmesi. Hook'lar ve component'ler HTTP detaylarını
 * bilmeden buradaki fonksiyonları çağırır.
 */
export const tasksClient = {
  getTasks: () => apiGet<Task[]>('/api/tasks'),

  // Gün 5+ eklenecek: createTask, updateTaskStatus, deleteTask
}
