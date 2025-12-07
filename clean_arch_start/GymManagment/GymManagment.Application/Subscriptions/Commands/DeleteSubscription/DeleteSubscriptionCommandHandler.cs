using ErrorOr;
using GymManagment.Application.Common.Interfaces;
using MediatR;

namespace GymManagment.Application.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler : IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
{
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdminsRepository _adminsRepository;
    // private readonly IGymsRepository _gymsRepository;

    public DeleteSubscriptionCommandHandler(
        ISubscriptionsRepository subscriptionsRepository,
        IUnitOfWork unitOfWork,
        IAdminsRepository adminsRepository
    )
    {
        _subscriptionsRepository = subscriptionsRepository;
        _unitOfWork = unitOfWork;
        _adminsRepository = adminsRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(command.SubscriptionId);
        if (subscription is null)
        {
            return Error.NotFound("Subscription not found");
        }

        var admin = await _adminsRepository.GetByIdAsync(subscription.AdminId);
        if (admin is null)
        {
            return Error.Unexpected(description: "Admin not found");
        }

        admin.DeleteSubscription(command.SubscriptionId);

        await _adminsRepository.UpdateAsync(admin);
        await _unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}