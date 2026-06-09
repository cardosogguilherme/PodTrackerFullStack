import { useState } from 'react';
import './App.css';
import { CreatePlayerForm } from './components/CreatePlayerForm';
import { PlayersList } from './components/PlayersList';
import { SearchPlayer } from './components/SeachPlayer';
import { usePlayers } from './hooks/usePlayers';

function App() {
  const { data: players, isLoading, error } = usePlayers();
  const [playerName, setPlayerName] = useState("");
  const filtered = (players ?? []).filter(p =>
    p.name.toLowerCase().includes(playerName.toLowerCase())
  );
  return (
    <>
      <CreatePlayerForm />
      <SearchPlayer value={playerName} onChange={setPlayerName} />
      <PlayersList players={filtered} isLoading={isLoading} error={error} />
    </>
  )
}

export default App
