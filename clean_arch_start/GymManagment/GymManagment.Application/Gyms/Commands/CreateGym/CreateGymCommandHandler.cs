using GymManagment.Domain.Gyms;
using MediatR;
using ErrorOr;
using GymManagment.Application.Common.Interfaces;

namespace GymManagment.Application.Gyms.Commands.CreateGym;

public class CreateGymCommandHandler : IRequestHandler<CreateGymCommand, ErrorOr<Gym>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public CreateGymCommandHandler(IUnitOfWork unitOfWork, IGymsRepository gymsRepository,
        ISubscriptionsRepository subscriptionsRepository)
    {
        _unitOfWork = unitOfWork;
        _gymsRepository = gymsRepository;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Gym>> Handle(CreateGymCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(request.SubscriptionId);
        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        var gym = new Gym(name: request.Name, maxRooms: subscription.GetMaxRooms(), subscriptionId: subscription.Id);
        var addGymResult = subscription.AddGym(gym);
        if (addGymResult.IsError)
        {
            return addGymResult.Errors;
        }

        await _subscriptionsRepository.UpdateAsync(subscription);
        await _gymsRepository.AddGymAsync(gym);
        await _unitOfWork.CommitChangesAsync();

        return gym;
    }
}