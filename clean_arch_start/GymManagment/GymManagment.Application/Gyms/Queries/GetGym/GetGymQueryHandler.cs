using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Gyms;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Gyms.Queries.GetGym;

public class GetGymQueryHandler : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public GetGymQueryHandler(IGymsRepository gymsRepository,
        ISubscriptionsRepository subscriptionsRepository)
    {
        _gymsRepository = gymsRepository;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Gym>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        if (await _subscriptionsRepository.ExistAsync(request.SubscriptionId))
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