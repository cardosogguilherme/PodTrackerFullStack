namespace PodTracker.Api.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Deck> Decks { get; set; } = new List<Deck>();
        public ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
    }
}