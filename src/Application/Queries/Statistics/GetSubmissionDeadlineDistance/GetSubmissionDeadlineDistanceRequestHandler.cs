using BldLeague.Application.Abstractions.Repositories;
using BldLeague.Application.Common;
using BldLeague.Application.Queries.Statistics.GetSolveDurationHistogram;
using MediatR;

namespace BldLeague.Application.Queries.Statistics.GetSubmissionDeadlineDistance;

public class GetSubmissionDeadlineDistanceRequestHandler(IUnitOfWork unitOfWork, RoundClock roundClock)
    : IRequestHandler<GetSubmissionDeadlineDistanceRequest, DeadlineDistanceHistogramDto>
{
    // 7 buckets weighted toward the deadline (max round length is 4 days).
    // Upper bounds in hours: 1, 2, 4, 8, 24, 48, 96.
    private static readonly (string Label, double UpperBoundHours)[] Boundaries =
    [
        ("<1h", 1),
        ("1–2h", 2),
        ("2–4h", 4),
        ("4–8h", 8),
        ("8–24h", 24),
        ("1–2 dni", 48),
        ("2–4 dni", 96),
    ];

    public async Task<DeadlineDistanceHistogramDto> Handle(GetSubmissionDeadlineDistanceRequest request, CancellationToken cancellationToken)
    {
        var localToday = roundClock.LocalToday();
        var pairs = await unitOfWork.StatisticsRepository.GetSubmissionWithDeadlineAsync(localToday);

        var counts = new int[Boundaries.Length];
        foreach (var (submittedAtUtc, endDate) in pairs)
        {
            var deadlineUtc = roundClock.LocalDayEndToUtc(endDate);
            var diff = deadlineUtc - submittedAtUtc;
            // Submissions where submittedAt > deadline are silently excluded.
            if (diff.Ticks <= 0)
                continue;

            var totalHours = diff.TotalHours;
            for (int i = 0; i < Boundaries.Length; i++)
            {
                if (totalHours < Boundaries[i].UpperBoundHours)
                {
                    counts[i]++;
                    break;
                }
            }
            // Submissions farther from the deadline than the largest bucket would be ignored,
            // but rounds cap at 4 days, so this branch is unreachable in practice.
        }

        var buckets = new HistogramBucketDto[Boundaries.Length];
        for (int i = 0; i < Boundaries.Length; i++)
            buckets[i] = new HistogramBucketDto(Boundaries[i].Label, counts[i]);

        return new DeadlineDistanceHistogramDto(buckets);
    }
}
