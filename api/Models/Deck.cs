namespace PodTracker.Api.Models;
public class Deck
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CommanderName { get; set; } = string.Empty;
    public int PlayerId { get; set; }

    public Player Player { get; set; } = null!;
}