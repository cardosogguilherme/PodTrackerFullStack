import { useState } from 'react';
import { useDecks } from '../hooks/useDecks';
import { usePlayers } from '../hooks/usePlayers';
import type { Participant } from '../models/Participant';

export function NewGameScreen() {
    const { data: players } = usePlayers();
    const { data: decks } = useDecks();
    const [participants, setParticipants] = useState<Participant[]>([
        { playerId: null, deckId: null },
        { playerId: null, deckId: null },   // seed the two-player minimum
    ]);

    const removePlayer = (index: number) => {
        if (participants.length <= 2) return;
        setParticipants(prev => prev.filter((_, i) => i !== index));
    }

    const addPlayer = (participant: Participant) => {
        if (participants.length === 6) return;

        setParticipants(prev => [...prev, participant])
    }

    const updatePlayer = (index: number, participant: Participant) => {
        setParticipants(prev => prev.map((slot, i) => i === index ? participant : slot))
    }

    return (
        <div>
            <h2>Create a new game</h2>
            {participants.map((slot, i) => (
                <div key={i}>
                    <select
                        value={slot.playerId ?? ''}
                        onChange={e => {
                            const playerId = e.target.value ? Number(e.target.value) : null;
                            updatePlayer(i, { playerId, deckId: null });
                        }}
                    >
                        <option value="">Select Player</option>
                        {(players ?? []).map(p => (
                            <option key={p.id} value={p.id}>{p.name}</option>
                        ))}
                    </select>

                    <select
                        value={slot.deckId ?? ''}
                        onChange={e => {
                            const deckId = e.target.value ? Number(e.target.value) : null;
                            updatePlayer(i, { ...slot, deckId });
                        }}
                    >
                        <option value="" disabled={slot.playerId === null}>Select Deck</option>
                        {(decks ?? []).filter(d => d.playerId === slot.playerId).map(p => (
                            <option key={p.id} value={p.id}>{p.name}</option>
                        ))}
                    </select>
                    <button onClick={() => removePlayer(i)}>Remove</button>
                </div>
            ))}

        </div>
    )
}