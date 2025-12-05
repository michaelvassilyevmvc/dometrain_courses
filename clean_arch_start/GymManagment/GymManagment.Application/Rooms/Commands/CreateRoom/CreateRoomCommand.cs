using GymManagment.Domain.Rooms;
using MediatR;
using ErrorOr;

namespace GymManagment.Application.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(Guid GymId, string RoomName) : IRequest<ErrorOr<Room>>;