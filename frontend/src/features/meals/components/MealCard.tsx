import { Link } from 'react-router-dom'
import type { MealSummary } from '../types'

interface MealCardProps {
  meal: MealSummary
}

export function MealCard({ meal }: MealCardProps) {
  return (
    <Link to={`/meals/${meal.id}`} className="meal-card">
      {meal.thumbnail && <img src={meal.thumbnail} alt={meal.name} loading="lazy" />}
      <div>
        <strong>{meal.name}</strong>
        <p className="meal-card__meta">
          {meal.category}
          {meal.area ? ` · ${meal.area}` : ''}
        </p>
      </div>
    </Link>
  )
}
