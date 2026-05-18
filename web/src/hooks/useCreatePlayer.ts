import { useMutation, useQuery } from "@tanstack/react-query";
import { API_BASE } from "../lib/api";
import { useQueryClient } from "@tanstack/react-query";

type CreatePlayerInput = {
    name: string;
}

export function useCreatePlayer() {
    const queryClient = useQueryClient();
    
    return useMutation({
        mutationFn: async (input: CreatePlayerInput) => {
            const response = await fetch(API_BASE + "/players", {
                method : "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(input)
            });

            if (!response.ok) {
                throw new Error('Failed to create player');
            }

            return await response.json();
        }, 
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['players'] })
        }
    });
}