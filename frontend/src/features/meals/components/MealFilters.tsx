import type { MealCategory } from '../types'

interface MealFiltersProps {
  category: string
  onCategoryChange: (value: string) => void
  categories: MealCategory[]
}

/**
 * GOLD referans: kategori filtresi. "Controlled input → query param → refetch" paterni.
 *
 * Stajyer genişletme noktaları (bilerek burada YOK):
 *  - S1-1 (rung-1): buraya debounce'lu bir "arama" input'u + "N sonuç" sayacı ekle.
 *    Backend zaten GET /api/meals?search=... destekliyor; sadece frontend'i bağla.
 *  - S2-1 (rung-1): buraya A-Z / Z-A sıralama düğmesi + "N sonuç" sayacı ekle.
 *  Bu bileşenin ve MealsPage'in category-filtresi paternini AYNALA.
 */
export function MealFilters({ category, onCategoryChange, categories }: MealFiltersProps) {
  return (
    <div className="searchbar">
      <select
        value={category}
        onChange={(e) => onCategoryChange(e.target.value)}
        aria-label="Kategori filtresi"
      >
        <option value="">Tüm kategoriler</option>
        {categories.map((c) => (
          <option key={c.name} value={c.name}>
            {c.name}
          </option>
        ))}
      </select>
    </div>
  )
}
