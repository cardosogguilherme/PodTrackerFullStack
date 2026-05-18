namespace PodTracker.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using PodTracker.Api.Models;
using PodTracker.Api.Data;
using PodTracker.Api.Dtos;

public static class DeckEndpoints
{
    public static void MapDeckEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/decks").WithTags("Decks");

        group.MapGet("/", async (PodTrackerDbContext db) => {
            var decks = await db.Decks.Select(deck => 
                new DeckResponse(
                    Id: deck.Id,
                    Name: deck.Name,
                    CommanderName: deck.CommanderName,
                    PlayerId: deck.PlayerId,
                    PlayerName: deck.Player.Name
                )
            ).ToListAsync();

            return Results.Ok(decks);
        });

        group.MapGet("/{id}", async (int id, PodTrackerDbContext db) =>
        {
            var deck = await db.Decks
            .Where(deck => deck.Id == id)
            .Select(deck => 
                new DeckResponse(
                    Id: deck.Id,
                    Name: deck.Name,
                    CommanderName: deck.CommanderName,
                    PlayerId: deck.PlayerId,
                    PlayerName: deck.Player.Name
                )
            ).FirstOrDefaultAsync();

            return deck is not null ? Results.Ok(deck) : Results.NotFound();
        });

        group.MapPost("/", async (CreateDeckRequest input, PodTrackerDbContext db) =>
        {
            var deck = new Deck 
            { 
                Name = input.Name, 
                CommanderName = input.CommanderName, 
                PlayerId = input.PlayerId 
            };
            db.Decks.Add(deck);
            await db.SaveChangesAsync();
            
            // Re-fetch and project to get PlayerName joined in
            var response = await db.Decks
                .Where(d => d.Id == deck.Id)
                .Select(d => new DeckResponse(d.Id, d.Name, d.CommanderName, d.PlayerId, d.Player.Name))
                .FirstAsync();
            
            return Results.Created($"/decks/{deck.Id}", response);
        });

        group.MapPut("/{id}", async (int id, CreateDeckRequest input, PodTrackerDbContext db) =>
        {
            var deck = await db.Decks.FindAsync(id);
            if (deck is null) { return Results.NotFound(); }

            deck.Name = input.Name;
            deck.CommanderName = input.CommanderName;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, PodTrackerDbContext db) =>
        {
            var deck = await db.Decks.FindAsync(id);
            if (deck is null) { return Results.NotFound(); }

            db.Decks.Remove(deck);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}