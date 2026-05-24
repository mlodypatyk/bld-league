using BldLeague.Application.Queries.Statistics.GetSolveDurationHistogram;

namespace BldLeague.Application.Queries.Statistics.GetSubmissionDeadlineDistance;

/// <summary>
/// Distance-to-deadline distribution: 7 buckets — <c>&lt;1h</c>, <c>1–2h</c>, <c>2–4h</c>, <c>4–8h</c>, <c>8–24h</c>, <c>1–2 dni</c>, <c>2–4 dni</c>.
/// </summary>
public record DeadlineDistanceHistogramDto(IReadOnlyList<HistogramBucketDto> Buckets);
