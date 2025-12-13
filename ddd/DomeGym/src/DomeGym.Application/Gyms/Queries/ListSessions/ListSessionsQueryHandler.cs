using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Gyms.Queries.ListSessions;

public class ListSessionsQueryHandler : IRequestHandler<ListSessionsQuery, ErrorOr<List<Session>>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IGymsRepository _gymsRepository;
    private readonly ISessionsRepository _sessionsRepository;

    public ListSessionsQueryHandler(
        ISubscriptionsRepository subscriptionsRepository,
        IGymsRepository gymsRepository,
        ISessionsRepository sessionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _gymsRepository = gymsRepository;
        _sessionsRepository = sessionsRepository;
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

        return await _sessionsRepository.ListByGymIdAsync(
            query.GymId,
            query.StartDateTime, query.EndDateTime,
            query.Categories);
    }
}