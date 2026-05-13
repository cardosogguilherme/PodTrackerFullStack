using PodTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace PodTracker.Api.Data;

public class PodTrackerDbContext : DbContext
{
    public PodTrackerDbContext(DbContextOptions<PodTrackerDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Deck> Decks => Set<Deck>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GamePlayer> GamePlayers => Set<GamePlayer>();
}