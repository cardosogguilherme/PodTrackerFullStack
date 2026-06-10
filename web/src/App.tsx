import { Link, Route, Routes } from 'react-router-dom';
import './App.css';
import { GameScreen } from './screens/GameScreen';
import { NewGameScreen } from './screens/NewGameScreen';
import { PlayersScreen } from './screens/PlayersScreen';

function App() {

  return (
    <>
      <nav>
        <Link to="/">Players</Link>
        <Link to="/games/new">New Game</Link>
      </nav>
      <Routes>
        <Route path="/" element={<PlayersScreen />} />
        <Route path="/games/new" element={<NewGameScreen />} />
        <Route path="/games/:id" element={<GameScreen />} />
      </Routes>
    </>
  )
}

export default App
