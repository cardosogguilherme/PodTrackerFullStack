namespace PodTracker.Api.Models
{
    public class GamePlayer
    {
        public int Id { get; set; }
        public int CurrentLife { get; set; }
        public int? Placement { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;


        public int DeckId { get; set; }
        public Deck Deck { get; set; } = null!;

    }
}