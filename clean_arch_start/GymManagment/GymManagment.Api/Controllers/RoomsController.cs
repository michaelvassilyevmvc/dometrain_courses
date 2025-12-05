using GymManagment.Application.Rooms.Commands.CreateRoom;
using GymManagment.Application.Rooms.Commands.DeleteRoom;
using GymManagment.Contracts.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.Api.Controllers;

[Route("gyms/{gymId}/rooms")]
public class RoomsController: ApiController
{
    private readonly ISender _mediator;

    public RoomsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateRoom(
        CreateRoomRequest request,
        Guid gymId)
    {
        // формируем команду на создание
        var command = new CreateRoomCommand(
            gymId,
            request.Name);

        // выполняем команду
        var createRoomResult = await _mediator.Send(command);

        // отправляем результат
        return createRoomResult.Match(
            room => Created(
                $"rooms/{room.Id}", // todo: add host
                new RoomResponse(room.Id, room.Name)),
            Problem);
    }

    [HttpDelete("{roomId:guid}")]
    public async Task<IActionResult> DeleteRoom(
        Guid gymId,
        Guid roomId)
    {
        // создаем команду на удаление
        var command = new DeleteRoomCommand(gymId, roomId);

        // выполняем ее
        var deleteRoomResult = await _mediator.Send(command);

        // отправляем результат
        return deleteRoomResult.Match(
            _ => NoContent(),
            Problem);
    }
}