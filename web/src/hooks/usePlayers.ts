import { useQuery } from '@tanstack/react-query';
import type { Player } from '../models/Player';
import { API_BASE } from "../lib/api";

async function fetchPlayers(signal: AbortSignal): Promise<Player[]> {
    const result = await fetch(API_BASE + '/players', { signal });

    if (!result.ok) { throw new Error('Failed to fetch players'); }
    const playersData: Player[] = await result.json();
    return playersData;
}

export function usePlayers() {
    return useQuery({
        queryKey: ['players'],
        queryFn: ({ signal }) => fetchPlayers(signal),
    });
}