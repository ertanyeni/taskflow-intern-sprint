import { Link, useParams } from 'react-router-dom'
import { AiSummaryPanel } from '../features/meals/components/AiSummaryPanel'
import { FavoriteButton } from '../features/meals/components/FavoriteButton'
import { useMeal } from '../features/meals/hooks/useMeal'

export function MealDetailPage() {
  const { id } = useParams<{ id: string }>()
  const { data: meal, isPending, isError, error } = useMeal(id)

  if (isPending) {
    return (
      <main>
        <p data-testid="loading">Yükleniyor…</p>
      </main>
    )
  }

  if (isError || !meal) {
    return (
      <main>
        <p role="alert">Tarif bulunamadı{error ? `: ${(error as Error).message}` : ''}.</p>
        <Link to="/meals">← Tariflere dön</Link>
      </main>
    )
  }

  return (
    <main>
      <Link to="/meals">← Tariflere dön</Link>
      <h1>{meal.name}</h1>
      <p className="meal-card__meta">
        {meal.category}
        {meal.area ? ` · ${meal.area}` : ''}
      </p>

      {meal.thumbnail && <img className="detail-img" src={meal.thumbnail} alt={meal.name} />}

      <div className="detail-actions">
        <FavoriteButton module="meals" externalId={meal.id} title={meal.name} thumbnail={meal.thumbnail} />
        {meal.youtube && (
          <a className="btn" href={meal.youtube} target="_blank" rel="noreferrer">
            ▶ Video
          </a>
        )}
      </div>

      {/* Rung-2 stajyer görevi: buraya türetilmiş alan(lar) gelecek
          (S1-2: IngredientCount, S2-2: HasVideo/InstructionWordCount). */}

      <h2>Malzemeler</h2>
      <ul>
        {meal.ingredients.map((ingredient) => (
          <li key={ingredient}>{ingredient}</li>
        ))}
      </ul>

      <h2>Hazırlanışı</h2>
      <p style={{ whiteSpace: 'pre-line' }}>{meal.instructions}</p>

      <h2>AI</h2>
      <AiSummaryPanel mealId={meal.id} />
    </main>
  )
}
