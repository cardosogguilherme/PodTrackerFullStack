export interface Participant {
    playerId: number | null
    deckId: number | null
}

export interface GameParticipant {
    playerId: number,
    playerName: string,
    deckName: string
    deckId: number,
    lifeTotal: number
}