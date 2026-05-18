import { useState } from 'react'
import './App.css'
import { PlayersList } from './components/PlayersList'
import { CreatePlayerForm } from './components/CreatePlayerForm'

function App() {
  return (
    <>
      <CreatePlayerForm/>
      <PlayersList />
    </>
  )
}

export default App
