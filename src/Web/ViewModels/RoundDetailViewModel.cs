using BldLeague.Application.Common;
using BldLeague.Application.Queries.Rounds.GetDetail;
using BldLeague.Web.Helpers;

namespace BldLeague.Web.ViewModels;

public class RoundDetailViewModel
{
    public required string StartDate { get; set; }
    public required string EndDate { get; set; }
    public required bool IsFinished { get; set; }
    public required List<string> Scrambles { get; set; }
    public required List<RoundStandingViewModel> Standings { get; set; }

    public static RoundDetailViewModel FromDto(RoundDetailDto dto, RoundClock clock)
    {
        return new RoundDetailViewModel
        {
            StartDate = dto.StartDate.ToDisplayDate(),
            EndDate = dto.EndDate.ToDisplayDate(),
            IsFinished = clock.IsRoundFinished(dto.EndDate),
            Scrambles = dto.Scrambles
                .OrderBy(s => s.ScrambleNumber)
                .Select(s => s.Notation)
                .ToList(),
            Standings = dto.Standings.Select(RoundStandingViewModel.FromDto).ToList()
        };
    }
}
