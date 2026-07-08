import { useTasks } from '../features/tasks/hooks/useTasks'
import { TaskList } from '../features/tasks/components/TaskList'
import { useState } from 'react'
export function TaskListPage() {
  const [status, setStatus] = useState('')
  const { data, isPending, isError, error } = useTasks()
  const visible = data
  ? status
    ? data.filter((t) => t.status === status)
    : data
  : []
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
<select
  value={status}
  onChange={(event) => setStatus(event.target.value)}
>
  <option value="">Hepsi</option>
  <option value="Todo">Todo</option>
  <option value="InProgress">InProgress</option>
  <option value="Done">Done</option>
</select>
<span>{visible.length} sonuç</span>
      {isPending && <p className="state" data-testid="loading">Yükleniyor…</p>}

      {isError && (
        <p className="state state--error" role="alert" data-testid="error">
          Task listesi yüklenemedi: {error.message}
        </p>
      )}

     {data && <TaskList tasks={visible} />}
    </main>
  )
}
