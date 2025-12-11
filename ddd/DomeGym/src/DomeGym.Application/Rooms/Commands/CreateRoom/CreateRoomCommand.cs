using DomeGym.Domain.RoomAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(Guid GymId, string RoomName) : IRequest<ErrorOr<Room>>;