export type TaskStatus = 'Todo' | 'InProgress' | 'Done'

export interface Task {
  id: string
  title: string
  status: TaskStatus
  createdAt: string
}
