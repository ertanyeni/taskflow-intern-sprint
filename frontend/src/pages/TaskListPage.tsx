import { useTasks } from '../features/tasks/hooks/useTasks'
import { TaskList } from '../features/tasks/components/TaskList'

export function TaskListPage() {
  const { data, isPending, isError, error } = useTasks()

  return (
    <main>
      <div className="page-head">
        <span className="kicker">TSK · 00 · Stajyer 2 ısınma alanı</span>
        <h1>Görevler</h1>
        <p>
          Basit iç görev listesi (owned veri, EF Core). Stajyer 2 rung 1-2'de burada çalışır:
          durum filtresi + sayaç, sonra türetilmiş bir alan.
        </p>
      </div>

      {isPending && <p className="state" data-testid="loading">Yükleniyor…</p>}

      {isError && (
        <p className="state state--error" role="alert" data-testid="error">
          Task listesi yüklenemedi: {error.message}
        </p>
      )}

      {data && <TaskList tasks={data} />}
    </main>
  )
}
