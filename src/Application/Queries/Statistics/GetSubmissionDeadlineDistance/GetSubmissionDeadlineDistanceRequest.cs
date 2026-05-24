using MediatR;

namespace BldLeague.Application.Queries.Statistics.GetSubmissionDeadlineDistance;

public record GetSubmissionDeadlineDistanceRequest : IRequest<DeadlineDistanceHistogramDto>;
