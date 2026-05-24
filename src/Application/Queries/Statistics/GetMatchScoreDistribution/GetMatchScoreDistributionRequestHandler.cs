using BldLeague.Application.Abstractions.Repositories;
using BldLeague.Application.Common;
using BldLeague.Application.Queries.Statistics.GetSolveDurationHistogram;
using MediatR;

namespace BldLeague.Application.Queries.Statistics.GetMatchScoreDistribution;

public class GetMatchScoreDistributionRequestHandler(IUnitOfWork unitOfWork, RoundClock roundClock)
    : IRequestHandler<GetMatchScoreDistributionRequest, ScoreDistributionDto>
{
    // 5 solves + 1 best-single bonus = 6 points per match.
    // Buckets are ordered pairs UserA:UserB from 0:6 .. 6:0 — 7 ordered pairs.
    private const int TotalPointsPerMatch = 6;

    public async Task<ScoreDistributionDto> Handle(GetMatchScoreDistributionRequest request, CancellationToken cancellationToken)
    {
        var localToday = roundClock.LocalToday();
        var scores = await unitOfWork.StatisticsRepository.GetFinishedMatchScoresAsync(localToday);

        var counts = new int[TotalPointsPerMatch + 1];
        foreach (var (scoreA, scoreB) in scores)
        {
            int a = Math.Clamp(scoreA, 0, TotalPointsPerMatch);
            counts[a]++;
        }

        var buckets = new HistogramBucketDto[TotalPointsPerMatch + 1];
        for (int a = 0; a <= TotalPointsPerMatch; a++)
        {
            int b = TotalPointsPerMatch - a;
            buckets[a] = new HistogramBucketDto($"{a}:{b}", counts[a]);
        }

        return new ScoreDistributionDto(buckets);
    }
}
