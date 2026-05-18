import { useState } from "react";
import { useCreatePlayer } from "../hooks/useCreatePlayer";

export function CreatePlayerForm() {
    const [playerName, setPlayerName] = useState("");
    const { mutate, isPending, error } = useCreatePlayer();

    return (
        <>
            {error && <p>{error.message}</p>}
            <form onSubmit={e =>  {
                e.preventDefault()
                
                if (!playerName.trim()) return
                
                mutate(
                    { name: playerName },
                    { onSuccess: () => setPlayerName("")}
                );
            }}>
                <input 
                    name="playerName" 
                    value={playerName}
                    disabled={isPending} 
                    onChange={e => setPlayerName(e.target.value)}
                />
                <button disabled={isPending} type="submit">Create Player</button>
            </form>
        </>
    );
}