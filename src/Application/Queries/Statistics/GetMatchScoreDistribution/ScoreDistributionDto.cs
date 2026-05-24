using BldLeague.Application.Queries.Statistics.GetSolveDurationHistogram;

namespace BldLeague.Application.Queries.Statistics.GetMatchScoreDistribution;

/// <summary>
/// Distribution of (UserAScore, UserBScore) pairs across finished matches.
/// Each match contributes one entry; total per match is 6 points.
/// </summary>
public record ScoreDistributionDto(IReadOnlyList<HistogramBucketDto> Buckets);
