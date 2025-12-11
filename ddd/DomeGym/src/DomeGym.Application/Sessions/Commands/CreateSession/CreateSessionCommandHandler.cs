using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.Common.ValueObjects;
using DomeGym.Domain.SessionAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Sessions.Commands.CreateSession;

public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, ErrorOr<Session>>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly ITrainersRepository _trainersRepository;

    public CreateSessionCommandHandler(IRoomsRepository roomsRepository, ITrainersRepository trainersRepository)
    {
        _roomsRepository = roomsRepository;
        _trainersRepository = trainersRepository;
    }

    public async Task<ErrorOr<Session>> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
    {
        var room = await _roomsRepository.GetByIdAsync(command.RoomId);
        if (room is null)
        {
            return Error.NotFound("Room not found");
        }

        var trainer = await _trainersRepository.GetByIdAsync(command.TrainerId);
        if(trainer is null)
        {
            return Error.NotFound("Trainer not found");
        }

        var createTimeRangeResult = TimeRange.FromDateTimes(command.StartDateTime, command.EndDateTime);
        if (createTimeRangeResult.IsError && createTimeRangeResult.FirstError.Type == ErrorType.Validation)
        {
            return Error.Validation("Invalid date and time");
        }

        if (!trainer.IsTimeSlotFree(DateOnly.FromDateTime(command.StartDateTime), createTimeRangeResult.Value))
        {
            return Error.Conflict("Trainer is not available at this time");
        }

        var session = new Session(
            name: command.Name,
            description: command.Description,
            maxParticipants: command.MaxParticipants,
            roomId: command.RoomId,
            trainerId: command.TrainerId,
            date: DateOnly.FromDateTime(command.StartDateTime),
            time: createTimeRangeResult.Value,
            categories: command.Categories
        );
        
        var scheduleSessionResult = room.ScheduleSession(session);
        if(scheduleSessionResult.IsError)
        {
            return scheduleSessionResult.Errors;
        }
        
        await _roomsRepository.UpdateAsync(room);
        
        return session;
    }
}