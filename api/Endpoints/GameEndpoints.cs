using Microsoft.EntityFrameworkCore;
using PodTracker.Api.Data;
using PodTracker.Api.Dtos;
using PodTracker.Api.Models;

namespace PodTracker.Api.Endpoints;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games").WithTags("Games");

        group.MapPost("/", async (StartGameRequest request, PodTrackerDbContext db) =>
        {
            var game = new Game
            {
                StartTime = DateTime.UtcNow,
                GamePlayers = request.Participants.Select(p => new GamePlayer
                {
                    PlayerId = p.PlayerId,
                    DeckId = p.DeckId,
                    CurrentLife = 40
                }).ToList()
            };

            db.Games.Add(game);
            await db.SaveChangesAsync();

            var response = await ProjectToResponse(db.Games.Where(g => g.Id == game.Id))
            .FirstAsync();

            return Results.Created($"/games/{game.Id}", response);
        });

        group.MapPut("/{id}/result", async (int id, FinishGameRequest request, PodTrackerDbContext db) =>
        {
            var game = await db.Games
                .Include(g => g.GamePlayers)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game is null)
            {
                return Results.NotFound();
            }

            var winner = game.GamePlayers.FirstOrDefault(gp => gp.Id == request.WinnerGamePlayerId);

            if (winner is null)
            {
                return Results.BadRequest($"WinnerGamePlayerId {request.WinnerGamePlayerId} is not a participant in this game.");
            }

            game.WinnerGamePlayerId = winner.Id;
            game.EndTime = DateTime.UtcNow;

            await db.SaveChangesAsync();

            var response = await ProjectToResponse(db.Games.Where(g => g.Id == game.Id))
            .FirstAsync();

            return Results.Ok(response);
        });
    }

    private static IQueryable<GameResponse> ProjectToResponse(IQueryable<Game> games) =>
        games.Select(g => new GameResponse(
                    Id: g.Id,
                    WinnerGamePlayerId: g.WinnerGamePlayerId,
                    StartTime: g.StartTime,
                    EndTime: g.EndTime,
                    Participants: g.GamePlayers.Select(gp => new GameParticipantResponse(
                        Id: gp.Id,
                        PlayerName: gp.Player.Name,
                        PlayerId: gp.PlayerId,
                        DeckId: gp.DeckId,
                        CommanderName: gp.Deck.CommanderName,
                        CurrentLife: gp.CurrentLife,
                        Placement: gp.Placement
                )).ToList()
            ));
}