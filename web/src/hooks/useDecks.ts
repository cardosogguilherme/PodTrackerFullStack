import { useQuery } from "@tanstack/react-query";
import { API_BASE } from "../lib/api";
import type { Deck } from "../models/Deck";

async function fetchDecks(signal: AbortSignal): Promise<Deck[]> {
    const result = await fetch(API_BASE + '/decks', { signal });

    if (!result.ok) { throw new Error('Failed to fetch decks') }

    const decksData: Deck[] = await result.json();
    return decksData;
}

export function useDecks() {
    return useQuery(
        {
            queryKey: ['decks'],
            queryFn: ({ signal }) => fetchDecks(signal)
        }
    );
}