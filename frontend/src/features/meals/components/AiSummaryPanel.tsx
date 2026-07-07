import { useAiSummary } from '../hooks/useAiSummary'

interface AiSummaryPanelProps {
  mealId: string
}

/**
 * AI özet/etiket paneli. Buton tetikler (mutation). Groq key yoksa backend 503 döner
 * ve buradaki error state'te anlamlı mesaj gösterilir (frontend key'i asla görmez).
 */
export function AiSummaryPanel({ mealId }: AiSummaryPanelProps) {
  const ai = useAiSummary(mealId)

  return (
    <section className="ai-panel">
      <button className="btn" onClick={() => ai.mutate()} disabled={ai.isPending}>
        {ai.isPending ? 'AI çalışıyor…' : '✨ AI özeti üret'}
      </button>

      {ai.isError && (
        <p role="alert" className="ai-panel__error">
          {(ai.error as Error).message}
        </p>
      )}

      {ai.data && (
        <div className="ai-panel__result">
          <p>{ai.data.summary}</p>
          <div className="tags">
            {ai.data.tags.map((tag) => (
              <span key={tag} className="tag">
                {tag}
              </span>
            ))}
          </div>
          <small>
            {ai.data.model}
            {ai.data.cached ? ' · cache' : ''}
          </small>
        </div>
      )}
    </section>
  )
}
