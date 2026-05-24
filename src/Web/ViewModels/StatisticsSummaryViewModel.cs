using BldLeague.Application.Queries.Statistics.GetStatisticsSummary;

namespace BldLeague.Web.ViewModels;

/// <summary>
/// Wraps <see cref="StatisticsSummaryDto"/> with the 3 home-page tiles ready to render.
/// </summary>
public class StatisticsSummaryViewModel
{
    public StatisticsSummaryViewModel(StatisticsSummaryDto summary)
    {
        Summary = summary;
        Tiles =
        [
            new StatTileViewModel(
                IconClass: "bi-puzzle",
                IconColorClass: "text-info",
                PrimaryText: $"{summary.ValidSolves} / {summary.Attempts}",
                Subtitle: "ułożone / próby"),
            new StatTileViewModel(
                IconClass: "bi-trophy",
                IconColorClass: "text-warning",
                PrimaryText: summary.Matches.ToString(),
                Subtitle: "rozegranych meczów"),
            new StatTileViewModel(
                IconClass: "bi-people-fill",
                IconColorClass: "text-success",
                PrimaryText: summary.Participants.ToString(),
                Subtitle: "zawodników"),
        ];
    }

    public StatisticsSummaryDto Summary { get; }
    public IReadOnlyList<StatTileViewModel> Tiles { get; }
}
