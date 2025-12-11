using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Participants.Queries.ListParticipantSessions;

public record ListParticipantSessionsQuery(Guid ParticipantId, DateTime? StartDateTime = null, DateTime? EndDateTime = null): IRequest<ErrorOr<List<Session>>>;