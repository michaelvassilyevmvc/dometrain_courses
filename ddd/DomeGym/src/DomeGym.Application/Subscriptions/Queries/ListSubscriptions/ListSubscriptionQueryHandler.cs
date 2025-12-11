using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.SubscriptionAggregate;
using MediatR;
using ErrorOr;
namespace DomeGym.Application.Subscriptions.Queries.ListSubscriptions;

public class ListSubscriptionQueryHandler: IRequestHandler<ListSubscriptionQuery, ErrorOr<List<Subscription>>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public ListSubscriptionQueryHandler(ISubscriptionsRepository subscriptionsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<List<Subscription>>> Handle(ListSubscriptionQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionsRepository.ListAsync();
    }
}