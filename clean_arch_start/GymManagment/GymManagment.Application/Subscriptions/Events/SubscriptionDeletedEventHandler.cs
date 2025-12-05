using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Admins.Events;
using MediatR;

namespace GymManagment.Application.Subscriptions.Events;

public class SubscriptionDeletedEventHandler: INotificationHandler<SubscriptionDeletedEvent>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionDeletedEventHandler(ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        SubscriptionDeletedEvent notification,
        CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(notification.SubscriptionId)
        ?? throw new InvalidOperationException("Subscription not found");

        await _subscriptionsRepository.RemoveSubscriptionAsync(subscription);
        await _unitOfWork.CommitChangesAsync();
    }
}