// Backend Api/Contracts karşılıkları. Alan isimleri backend record'larıyla birebir.

export interface MealSummary {
  id: string
  name: string
  category: string
  area: string
  thumbnail: string
}

export interface MealDetail {
  id: string
  name: string
  category: string
  area: string
  instructions: string
  thumbnail: string
  youtube: string | null
  ingredients: string[]
  tags: string[]
}

export interface MealCategory {
  name: string
  thumbnail: string | null
}

export interface Favorite {
  id: string
  module: string
  externalId: string
  title: string
  thumbnail: string | null
  createdAt: string
}

export interface AiSummary {
  summary: string
  tags: string[]
  model: string
  cached: boolean
}

export interface CreateFavorite {
  module: string
  externalId: string
  title: string
  thumbnail: string | null
}
