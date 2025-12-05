using GymManagment.Domain.Admins;

namespace GymManagment.Application.Common.Interfaces;

public interface IAdminsRepository
{
    Task<Admin?> GetByIdAsync(Guid adminId);
    Task UpdateAsync(Admin admin);
}