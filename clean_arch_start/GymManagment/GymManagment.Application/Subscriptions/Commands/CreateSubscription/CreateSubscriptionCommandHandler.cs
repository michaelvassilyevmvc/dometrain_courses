using MediatR;
using ErrorOr;
using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Subscriptions;

namespace GymManagment.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    // private readonly IUnitOfWork _unitOfWork;

    public CreateSubscriptionCommandHandler(
        ISubscriptionsRepository subscriptionsRepository
    )
    {
        _subscriptionsRepository = subscriptionsRepository;
        // _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        // Create Subscription
        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            SubcriptionType = request.SubcriptionType
        };

        // Add Subscription to DB
        await _subscriptionsRepository.AdbSubscriptionAsync(subscription);

        // await _unitOfWork.CommitChangesAsync();
        // return Subscription
        return subscription;
    }
}