using DomeGym.Application.Common.Interfaces;
using DomeGym.Domain.RoomAggregate;
using Microsoft.EntityFrameworkCore;

namespace DomeGym.Infrastructure.Persistence.Repositories;

public class RoomsRepository: IRoomsRepository
{
    private readonly DomeGymDbContext _dbContext;
    
    public RoomsRepository(DomeGymDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddRoomAsync(Room room)
    {
        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid roomId)
    {
        return await _dbContext.Rooms.AsNoTracking().FirstOrDefaultAsync(x => x.Id == roomId);
    }

    public async Task<List<Room>> ListByGymIdAsync(Guid gymId)
    {
        return await _dbContext.Rooms.AsNoTracking().Where(x => x.GymId == gymId).ToListAsync();
    }

    public async Task RemoveAsync(Room room)
    {
        _dbContext.Rooms.Remove(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _dbContext.Rooms.Update(room);
        await _dbContext.SaveChangesAsync();
    }
}