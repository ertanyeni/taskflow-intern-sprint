import { Route, Routes } from 'react-router-dom'
import { NavBar } from './NavBar'
import { HomePage } from '../pages/HomePage'
import { TaskListPage } from '../pages/TaskListPage'
import { MealsPage } from '../pages/MealsPage'
import { MealDetailPage } from '../pages/MealDetailPage'
import { FavoritesPage } from '../pages/FavoritesPage'

export function App() {
  return (
    <>
      <NavBar />
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/tasks" element={<TaskListPage />} />
        <Route path="/meals" element={<MealsPage />} />
        <Route path="/meals/:id" element={<MealDetailPage />} />
        <Route path="/favorites" element={<FavoritesPage />} />
        {/* Stajyer modülleri buraya eklenecek:
            <Route path="/pokedex" ... />  (Stajyer 1, S1-3)
            <Route path="/library" ... />  (Stajyer 2, S2-3) */}
      </Routes>
    </>
  )
}
