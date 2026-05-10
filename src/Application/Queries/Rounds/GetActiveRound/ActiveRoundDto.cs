namespace BldLeague.Application.Queries.Rounds.GetActiveRound;

public class ActiveRoundDto
{
    public Guid RoundId { get; init; }
    public int RoundNumber { get; init; }
    public int SeasonNumber { get; init; }
    public DateTime EndsAtUtc { get; init; }
}
