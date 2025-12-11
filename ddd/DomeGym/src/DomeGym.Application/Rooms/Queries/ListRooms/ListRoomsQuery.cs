using DomeGym.Domain.RoomAggregate;
using MediatR;
using ErrorOr;
namespace DomeGym.Application.Rooms.Queries.ListRooms;

public record ListRoomsQuery(Guid GymId):IRequest<ErrorOr<List<Room>>>;