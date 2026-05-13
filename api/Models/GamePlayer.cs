namespace PodTracker.Api.Models
{
    public class GamePlayer
    {
        public int Id { get; set; }
        public Game Game { get; set; } = null!;
        public Player Player { get; set; } = null!;
        public Deck Deck { get; set; } = null!;
        public int CurrentLife { get; set; }
        public int? Placement { get; set; }
    }   
}