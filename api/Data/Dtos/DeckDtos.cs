namespace PodTracker.Api.Dtos;

public record DeckResponse(
    int Id,
    string Name,
    string CommanderName,
    int PlayerId,
    string PlayerName
);

public record CreateDeckRequest(
    string Name,
    string CommanderName,
    int PlayerId
);