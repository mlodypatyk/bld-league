using MediatR;

namespace BldLeague.Application.Queries.Statistics.GetSubmissionHourHistogram;

public record GetSubmissionHourHistogramRequest : IRequest<SubmissionHourHistogramDto>;
