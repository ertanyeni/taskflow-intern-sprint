import type { MealSummary } from '../types'
import { MealCard } from './MealCard'

interface MealListProps {
  meals: MealSummary[]
}

/** Tasks'taki TaskList ile aynı fikir: liste boşsa empty state, doluysa kartlar. */
export function MealList({ meals }: MealListProps) {
  if (meals.length === 0) {
    return <p data-testid="empty-state">Sonuç yok. Aramayı veya kategoriyi değiştir.</p>
  }

  return (
    <div className="meal-grid">
      {meals.map((meal) => (
        <MealCard key={meal.id} meal={meal} />
      ))}
    </div>
  )
}
