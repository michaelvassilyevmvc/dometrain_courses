using ErrorOr;
using MediatR;

namespace GymManagment.Application.Rooms.Commands.DeleteRoom;

public record DeleteRoomCommand(Guid GymId, Guid RoomId): IRequest<ErrorOr<Deleted>>;