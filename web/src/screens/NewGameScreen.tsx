import { useParams } from 'react-router-dom';
export function NewGameScreen() {
    const { id } = useParams();
    return <div>Game {id}</div>;
}