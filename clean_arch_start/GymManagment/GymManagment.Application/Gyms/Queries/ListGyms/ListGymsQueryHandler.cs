using GymManagment.Application.Common.Interfaces;
using ErrorOr;
using GymManagment.Domain.Gyms;
using MediatR;

namespace GymManagment.Application.Gyms.Queries.ListGyms;

public class ListGymsQueryHandler: IRequestHandler<ListGymsQuery, ErrorOr<List<Gym>>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public ListGymsQueryHandler(IGymsRepository gymsRepository,
        ISubscriptionsRepository subscriptionsRepository)
    {
        _gymsRepository = gymsRepository;
        _subscriptionsRepository = subscriptionsRepository;
    }


    public async Task<ErrorOr<List<Gym>>> Handle(ListGymsQuery request, CancellationToken cancellationToken)
    {
        // проверка на существование
        if (!await _subscriptionsRepository.ExistAsync(request.SubscriptionId))
        {
            return Error.NotFound("Subscription not found");
        }

        return await _gymsRepository.ListBySubscriptionIdAsync(request.SubscriptionId);
    }
}