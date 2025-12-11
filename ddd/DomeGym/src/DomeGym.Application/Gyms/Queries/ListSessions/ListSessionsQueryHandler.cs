using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Gyms.Queries.ListSessions;

public class ListSessionsQueryHandler : IRequestHandler<ListSessionsQuery, ErrorOr<List<Session>>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IGymsRepository _gymsRepository;
    private readonly ISessionRepository _sessionRepository;

    public ListSessionsQueryHandler(
        ISubscriptionsRepository subscriptionsRepository,
        IGymsRepository gymsRepository,
        ISessionRepository sessionRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _gymsRepository = gymsRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task<ErrorOr<List<Session>>> Handle(ListSessionsQuery query, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(query.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound("Subscription not found");
        }

        if (!subscription.HasGym(query.GymId))
        {
            return Error.NotFound("Gym not found");
        }

        return await _sessionRepository.ListByGymIdAsync(
            query.GymId,
            query.StartDateTime, query.EndDateTime,
            query.Categories);
    }
}