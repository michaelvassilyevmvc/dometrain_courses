using MediatR;
using ErrorOr;
namespace DomeGym.Application.Rooms.Commands.DeleteRoom;

public record DeleteRoomCommand(Guid GymId, Guid RoomId): IRequest<ErrorOr<Deleted>>;