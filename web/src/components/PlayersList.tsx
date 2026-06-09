import type { Player } from "../models/Player";

type PlayersProps = {
    players: Player[],
    isLoading: boolean,
    error: Error | null
}

export function PlayersList({ players: players, isLoading, error }: PlayersProps) {
    // const { data: players, isLoading, error } = usePlayers();
    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error while loading</div>;
    }

    if (!players || players.length === 0) {
        return <div>No players available</div>
    }

    return (
        <ul>
            {players.map(player =>
                <li key={player.id}>{player.name}</li>
            )}
        </ul>
    );
}