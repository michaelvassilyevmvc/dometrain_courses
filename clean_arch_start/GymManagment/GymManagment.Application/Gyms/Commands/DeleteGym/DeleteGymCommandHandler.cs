using ErrorOr;
using GymManagment.Application.Common.Interfaces;
using MediatR;

namespace GymManagment.Application.Gyms.Commands.DeleteGym;

public class DeleteGymCommandHandler: IRequestHandler<DeleteGymCommand, ErrorOr<Deleted>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubscriptionsRepository _subscriptionsRepository;

    public DeleteGymCommandHandler(IGymsRepository gymsRepository, IUnitOfWork unitOfWork, ISubscriptionsRepository subscriptionsRepository)
    {
        _gymsRepository = gymsRepository;
        _unitOfWork = unitOfWork;
        _subscriptionsRepository = subscriptionsRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteGymCommand request, CancellationToken cancellationToken)
    {
        // проверка gym
        var gym = await _gymsRepository.GetByIdAsync(request.GymId);
        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }
        
        // проверка subscription
        var subscription = await _subscriptionsRepository.GetByIdAsync(gym.SubscriptionId);
        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }
        
        // проверка есть ли gym в подписках
        if(!subscription.HasGym(gym.Id))
        {
            return Error.NotFound(description: "Gym not found in subscription");
        }
        
        // удалить gym из subscription
        subscription.RemoveGym(gym.Id);
        
        // обновление локально, сохранение в БД
        await _subscriptionsRepository.UpdateAsync(subscription);
        await _gymsRepository.RemoveAsync(gym);
        await _unitOfWork.CommitChangesAsync();
        
        // вывод результат
        return Result.Deleted;
    }
}