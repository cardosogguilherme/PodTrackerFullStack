namespace PodTracker.Api.Dtos;

public record GameParticipant(
    int PlayerId,
    int DeckId
);

public record StartGameRequest(List<GameParticipant> Participants);

public record FinishGameRequest(int WinnerGamePlayerId);

public record GameParticipantResponse(
    int Id,
    string PlayerName,
    int PlayerId,
    int DeckId,
    string CommanderName,
    int CurrentLife,
    int? Placement
);

public record GameResponse(
    int Id,
    int? WinnerGamePlayerId,
    DateTime StartTime,
    DateTime? EndTime,
    List<GameParticipantResponse> Participants
);

public record StatsResponse(
    int GamesPlayed,
    int Wins,
    double WinRatio
);