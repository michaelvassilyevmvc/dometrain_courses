using MediatR;
using ErrorOr;
using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Subscriptions;

namespace GymManagment.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdminsRepository _adminsRepository;

    public CreateSubscriptionCommandHandler(
        ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork, IAdminsRepository adminsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _unitOfWork = unitOfWork;
        _adminsRepository = adminsRepository;
    }

    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await _adminsRepository.GetByIdAsync(request.AdminId);
        if (admin is null)
        {
            return Error.NotFound(description: "Admin not found");
        }
        // Create Subscription
        var subscription = new Subscription
        (
            subscriptionType: request.SubcriptionType,
            adminId: request.AdminId
        );
        
        if(admin.SubscriptionId is not null)
        {
            return Error.Conflict(description: "Admin already has a subscription");
        }
        
        admin.SetSubscription(subscription);

        // Add Subscription to DB
        await _subscriptionsRepository.AdbSubscriptionAsync(subscription);
        await _adminsRepository.UpdateAsync(admin);
        await _unitOfWork.CommitChangesAsync();

        return subscription;
    }
}