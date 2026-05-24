namespace BldLeague.Application.Queries.Statistics.GetSubmissionHourHistogram;

/// <summary>
/// Submission counts bucketed by hour-of-day (0..23) in the configured league time zone.
/// </summary>
/// <param name="Counts">Counts per hour bucket. Always 24 elements (index = local hour).</param>
/// <param name="IncludedMatches">Finished matches with at least one non-null submission timestamp.</param>
/// <param name="TotalMatches">Total finished matches in scope.</param>
public record SubmissionHourHistogramDto(
    IReadOnlyList<int> Counts,
    int IncludedMatches,
    int TotalMatches);
