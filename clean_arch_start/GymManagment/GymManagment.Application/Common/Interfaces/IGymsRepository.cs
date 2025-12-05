using GymManagment.Domain.Gyms;

namespace GymManagment.Application.Common.Interfaces;

public interface IGymsRepository
{
    Task<Gym?> GetByIdAsync(Guid id);
    Task<List<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId);
    Task<bool> ExistAsync(Guid id);
    Task AddGymAsync(Gym gym);
    Task UpdateAsync(Gym gym);
    Task RemoveAsync(Gym gym);
    Task RemoveRangeAsync(List<Gym> gyms);
    
}