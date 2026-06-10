import { useParams } from 'react-router-dom';
export function GameScreen() {
    const { id } = useParams();
    return <div>Game {id}</div>;
}

export function ParticipantSlot({ participant, onLifeChange }: ParticipantSlotProps) {
    return (
        <>
            <div>{participant.playerName}</div>
            <div>Playing {participant.deckName}</div>
            <button>-</button><h3>{participant.lifeTotal}</h3><button>+</button>
        </>
    );
}