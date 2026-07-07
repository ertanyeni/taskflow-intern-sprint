import { useState } from 'react'
import { MealFilters } from '../features/meals/components/MealFilters'
import { MealList } from '../features/meals/components/MealList'
import { useMealCategories } from '../features/meals/hooks/useMealCategories'
import { useMeals } from '../features/meals/hooks/useMeals'

export function MealsPage() {
  // UI state (filtre) burada; server state (liste) React Query'de.
  const [category, setCategory] = useState('')

  const { data: categories } = useMealCategories()
  // NOT: search boş — gold sadece kategori filtresi kullanıyor.
  // Rung-1 stajyer görevi search'ü bağlayacak (useMeals ilk parametresi).
  const { data: meals, isPending, isError, error } = useMeals('', category)

  return (
    <main>
      <h1>Tarifler</h1>
      <p>Dış API (TheMealDB) + cache + detay + favori + AI — gold referans modül.</p>

      <MealFilters category={category} onCategoryChange={setCategory} categories={categories ?? []} />

      {isPending && <p data-testid="loading">Yükleniyor…</p>}
      {isError && (
        <p role="alert" data-testid="error">
          Tarifler yüklenemedi: {(error as Error).message}
        </p>
      )}
      {meals && <MealList meals={meals} />}
    </main>
  )
}
