namespace BldLeague.Application.Queries.Statistics.GetStatisticsSummary;

/// <summary>
/// Global counters for the home page and statistics page header tiles.
/// </summary>
/// <param name="ValidSolves">Count of valid (non-DNF, non-DNS) solves on finished matches.</param>
/// <param name="Attempts">Valid + DNF solves on finished matches (DNS excluded).</param>
/// <param name="Matches">Count of finished matches, including BYE matches.</param>
/// <param name="Participants">Count of distinct users that have at least one finished-match appearance.</param>
public record StatisticsSummaryDto(int ValidSolves, int Attempts, int Matches, int Participants);
