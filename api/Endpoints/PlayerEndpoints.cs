using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PodTracker.Api.Models;
using PodTracker.Api.Data;
using PodTracker.Api.Dtos;

namespace PodTracker.Api.Endpoints;

public static class PlayerEndpoints
{
    public static void MapPlayerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/players").WithTags("Players");

        group.MapGet("/", async (PodTrackerDbContext db) => await db.Players.ToListAsync());

        group.MapGet("/{id}", async (int id, PodTrackerDbContext db) =>
        {
            var player = await db.Players.FindAsync(id);
            return player is not null ? Results.Ok(player) : Results.NotFound();
        });

        group.MapPost("/", async (Player player, PodTrackerDbContext db) =>
        {
            db.Players.Add(player);
            await db.SaveChangesAsync();

            return Results.Created($"/players/{player.Id}", player);
        });

        group.MapPut("/{id}", async (int id, Player input, PodTrackerDbContext db) =>
        {
            var player = await db.Players.FindAsync(id);
            if (player is null) { return Results.NotFound(); }

            player.Name = input.Name;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, PodTrackerDbContext db) =>
        {
            var player = await db.Players.FindAsync(id);
            if (player is null) { return Results.NotFound(); }

            db.Players.Remove(player);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapGet("{id}/stats/", async (int id, PodTrackerDbContext db) =>
        {
            var stats = await db.Players
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    Played = p.GamePlayers.Count(gp => gp.Game.EndTime != null),
                    Wins = p.GamePlayers.Count(gp =>
                        gp.Game.EndTime != null && gp.Game.WinnerGamePlayerId == gp.Id)
                }).FirstOrDefaultAsync();

            if (stats is null) return Results.NotFound();

            return Results.Ok(new StatsResponse(
                    GamesPlayed: stats.Played,
                    Wins: stats.Wins,
                    WinRatio: stats.Played > 0 ? Math.Round((double)stats.Wins / stats.Played, 2) : 0.0
                ));
        });
    }
}