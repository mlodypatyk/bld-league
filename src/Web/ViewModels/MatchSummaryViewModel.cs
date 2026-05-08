using BldLeague.Application.Common;
using BldLeague.Application.Queries.Matches.GetMatchSummaries;

namespace BldLeague.Web.ViewModels;

public class MatchSummaryViewModel
{
    public Guid MatchId { get; set; }
    public Guid UserAId { get; set; }
    public Guid? UserBId { get; set; }
    public required string UserAFullName { get; set; }
    public string? UserBFullName { get; set; }
    public int UserAScore { get; set; }
    public int UserBScore { get; set; }
    public MatchStatus Status { get; set; }

    protected MatchSummaryViewModel() {}

    public static MatchSummaryViewModel FromDto(MatchSummaryDto dto, RoundClock clock)
    {
        return new MatchSummaryViewModel
        {
            MatchId = dto.Id,
            UserAId = dto.UserAId,
            UserBId = dto.UserBId,
            UserAFullName = dto.UserAFullName,
            UserBFullName = dto.UserBFullName,
            UserAScore = dto.UserAScore,
            UserBScore = dto.UserBScore,
            Status = ComputeStatus(clock, dto.RoundStartDate, dto.RoundEndDate, dto.BothSidesSubmitted),
        };
    }

    protected static MatchStatus ComputeStatus(RoundClock clock, DateTime startDate, DateTime endDate, bool bothSidesSubmitted)
    {
        if (clock.IsRoundFinished(endDate) || bothSidesSubmitted)
            return MatchStatus.Finished;
        if (clock.IsRoundActive(startDate, endDate))
            return MatchStatus.InProgress;
        return MatchStatus.Upcoming;
    }
}
