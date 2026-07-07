import type { Task } from '../types'

interface TaskListProps {
  tasks: Task[]
}

export function TaskList({ tasks }: TaskListProps) {
  if (tasks.length === 0) {
    return (
      <p className="state" data-testid="empty-state">
        Henüz task yok. Task oluşturma (CreateTaskForm) Gün 5 referans modülünde eklenecek.
      </p>
    )
  }

  return (
    <ul className="content-list">
      {tasks.map((task) => (
        <li key={task.id}>
          <strong>{task.title}</strong> — {task.status}
        </li>
      ))}
    </ul>
  )
}
