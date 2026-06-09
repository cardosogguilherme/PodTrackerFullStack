namespace PodTracker.Api.Dtos;

public record PlayerResponse(
    int Id,
    string Name
);

public record CreatePlayerRequest(
    string Name
);

public record UpdatePlayerRequest(
    int Id,
    string Name
);