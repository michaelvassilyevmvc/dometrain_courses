using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.GymAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Gyms.Queries.GetGym;

public class GetGymQueryHandler : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public GetGymQueryHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _gymsRepository = gymsRepository;
    }

    public async Task<ErrorOr<Gym>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        if (await _subscriptionsRepository.ExistsAsync(request.SubscriptionId))
        {
            return Error.NotFound("Subscription not found");
        }

        if (await _gymsRepository.GetByIdAsync(request.GymId) is not Gym gym)
        {
            return Error.NotFound("Gym not found");
        }

        return gym;
    }
}