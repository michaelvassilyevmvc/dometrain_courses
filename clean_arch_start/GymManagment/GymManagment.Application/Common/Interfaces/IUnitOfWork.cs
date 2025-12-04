namespace GymManagment.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}