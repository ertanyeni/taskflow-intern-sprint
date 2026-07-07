import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { describe, it, expect } from 'vitest'
import { MealList } from './MealList'
import type { MealSummary } from '../types'

// MealCard <Link> kullandığı için testleri router context'i içinde render ediyoruz.
function renderInRouter(ui: React.ReactNode) {
  return render(<MemoryRouter>{ui}</MemoryRouter>)
}

const sample: MealSummary = {
  id: '1',
  name: 'Test Tarifi',
  category: 'Chicken',
  area: 'Turkish',
  thumbnail: '',
}

describe('MealList', () => {
  it('liste boşken empty state gösterir', () => {
    renderInRouter(<MealList meals={[]} />)
    expect(screen.getByTestId('empty-state')).toBeInTheDocument()
  })

  it('meal varken kartı listeler', () => {
    renderInRouter(<MealList meals={[sample]} />)
    expect(screen.getByText('Test Tarifi')).toBeInTheDocument()
  })
})
