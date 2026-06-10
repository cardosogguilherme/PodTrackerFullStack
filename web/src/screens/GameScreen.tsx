import { useParams } from 'react-router-dom';
export function GameScreen() {
    const { id } = useParams();
    return <div>Game {id}</div>;
}