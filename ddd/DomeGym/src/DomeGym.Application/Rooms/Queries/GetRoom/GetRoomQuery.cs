using DomeGym.Domain.RoomAggregate;
using MediatR;
using ErrorOr;

namespace DomeGym.Application.Rooms.Queries.GetRoom;

public record GetRoomQuery(Guid GymId, Guid RoomId):IRequest<ErrorOr<Room>>;