using BldLeague.Application.Abstractions.Repositories;
using BldLeague.Application.Common;
using MediatR;

namespace BldLeague.Application.Queries.Statistics.GetSubmissionHourHistogram;

public class GetSubmissionHourHistogramRequestHandler(IUnitOfWork unitOfWork, RoundClock roundClock)
    : IRequestHandler<GetSubmissionHourHistogramRequest, SubmissionHourHistogramDto>
{
    public async Task<SubmissionHourHistogramDto> Handle(GetSubmissionHourHistogramRequest request, CancellationToken cancellationToken)
    {
        var localToday = roundClock.LocalToday();
        var rawSubmissions = await unitOfWork.StatisticsRepository.GetSubmissionTimestampsAsync(localToday);

        var counts = new int[24];
        foreach (var utc in rawSubmissions.Timestamps)
        {
            var local = roundClock.ToLocal(utc);
            counts[local.Hour]++;
        }

        return new SubmissionHourHistogramDto(counts, rawSubmissions.IncludedMatches, rawSubmissions.TotalMatches);
    }
}
