using ErrorOr;
using GymManagment.Application.Common.Interfaces;
using MediatR;

namespace GymManagment.Application.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler: IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdminsRepository _adminsRepository;
    private readonly IGymsRepository _gymsRepository;

    public DeleteSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork, IAdminsRepository adminsRepository, IGymsRepository gymsRepository)
    {
        _subscriptionsRepository = subscriptionsRepository;
        _unitOfWork = unitOfWork;
        _adminsRepository = adminsRepository;
        _gymsRepository = gymsRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(command.SubscriptionId);
        if(subscription is null)
        {
            return Error.NotFound("Subscription not found");
        }

        var admin = await _adminsRepository.GetByIdAsync(subscription.AdminId);
        if (admin is null)
        {
            return Error.Unexpected(description:"Admin not found");
        }
        
        admin.DeleteSubscription(command.SubscriptionId);
        var gymsToDelete = await _gymsRepository.ListBySubscriptionIdAsync(command.SubscriptionId);
        
        await _adminsRepository.UpdateAsync(admin);
        await _subscriptionsRepository.RemoveSubscriptionAsync(subscription);
        await _gymsRepository.RemoveRangeAsync(gyms: gymsToDelete);
        await _unitOfWork.CommitChangesAsync();
        
        return Result.Deleted;
    }
}