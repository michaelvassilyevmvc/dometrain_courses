using DomeGym.Domain;
using DomeGym.Domain.SessionAggregate;

namespace DomeGym.Application.Interfaces;

public interface ISessionRepository
{
    Task AddSessionAsync(Session session);
    Task UpdateSessionAsync(Session session);
}