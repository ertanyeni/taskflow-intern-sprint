import { useTasks } from '../features/tasks/hooks/useTasks'
import { TaskList } from '../features/tasks/components/TaskList'

export function TaskListPage() {
  const { data, isPending, isError, error } = useTasks()

  return (
    <main>
      <h1>TaskFlow</h1>
      <p>Küçük ekipler için görev takip uygulaması — MVP iskeleti.</p>

      {isPending && <p data-testid="loading">Yükleniyor…</p>}

      {isError && (
        <p role="alert" data-testid="error">
          Task listesi yüklenemedi: {error.message}
        </p>
      )}

      {data && <TaskList tasks={data} />}
    </main>
  )
}
