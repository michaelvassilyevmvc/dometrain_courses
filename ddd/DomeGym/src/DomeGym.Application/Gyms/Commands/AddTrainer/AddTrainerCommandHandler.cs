using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.TrainerAggregate;
using MediatR;
    using ErrorOr;
namespace DomeGym.Application.Gyms.Commands.AddTrainer;

public class AddTrainerCommandHandler: IRequestHandler<AddTrainerCommand, ErrorOr<Success>>
{
    private readonly IGymsRepository _gymsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository;
    private readonly ITrainersRepository _trainersRepository;

    public AddTrainerCommandHandler(IGymsRepository gymsRepository, ISubscriptionsRepository subscriptionsRepository, ITrainersRepository trainersRepository)
    {
        _gymsRepository = gymsRepository;
        _subscriptionsRepository = subscriptionsRepository;
        _trainersRepository = trainersRepository;
    }

    public async  Task<ErrorOr<Success>> Handle(AddTrainerCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(command.SubscriptionId);
        if (subscription is null)
        {
            return Error.NotFound("Subscription not found");
        }

        if (!subscription.HasGym(command.GymId))
        {
            return Error.NotFound("Gym not found");
        }
        
        var gym = await _gymsRepository.GetByIdAsync(command.GymId);
        if (gym is null)
        {
            return Error.NotFound("Gym not found");
        }

        if (gym.HasTrainer(command.TrainerId))
        {
            return Error.Conflict("Trainer already added");
        }

        Trainer? trainer = await _trainersRepository.GetByIdAsync(command.TrainerId);
        if (trainer is null)
        {
            return Error.NotFound("Trainer not found");
        }
        
        gym.AddTrainer(trainer);
        await _gymsRepository.UpdateAsync(gym);
        return Result.Success;
    }
}