import { render, screen } from '@testing-library/react'
import { describe, it, expect } from 'vitest'
import { TaskList } from './TaskList'

describe('TaskList', () => {
  it('liste boşken empty state gösterir', () => {
    render(<TaskList tasks={[]} />)

    expect(screen.getByTestId('empty-state')).toBeInTheDocument()
  })

  it('task varken her task’ı listeler', () => {
    render(
      <TaskList
        tasks={[
          { id: '1', title: 'İlk task', status: 'Todo', createdAt: '2026-01-01T00:00:00Z' },
        ]}
      />,
    )

    expect(screen.getByText('İlk task')).toBeInTheDocument()
  })
})
