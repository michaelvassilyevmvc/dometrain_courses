using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Admins.Events;
using MediatR;

namespace GymManagment.Application.Gyms.Events;

public class SubscriptionDeletedEventHandler: INotificationHandler<SubscriptionDeletedEvent>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionDeletedEventHandler(
         IUnitOfWork unitOfWork, IGymsRepository gymsRepository)
    {
        _unitOfWork = unitOfWork;
        _gymsRepository = gymsRepository;
    }

    public async Task Handle(
        SubscriptionDeletedEvent notification,
        CancellationToken cancellationToken)
    {
        var gyms = await _gymsRepository.ListBySubscriptionIdAsync(notification.SubscriptionId);

        await _gymsRepository.RemoveRangeAsync(gyms);
        await _unitOfWork.CommitChangesAsync();
    }
}