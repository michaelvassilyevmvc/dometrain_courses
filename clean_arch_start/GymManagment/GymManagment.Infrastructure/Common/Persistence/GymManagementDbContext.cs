using System.Reflection;
using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Admins;
using GymManagment.Domain.Common;
using GymManagment.Domain.Gyms;
using GymManagment.Domain.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Infrastructure.Common.Persistence;

public class GymManagementDbContext : DbContext, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublisher _publisher;
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Gym> Gyms { get; set; } = null!;

    public GymManagementDbContext(
        DbContextOptions<GymManagementDbContext> options,
        IHttpContextAccessor httpContextAccessor, IPublisher publisher) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _publisher = publisher;
    }

    public async Task CommitChangesAsync()
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(events => events)
            .ToList();

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            await PublishDomainEvents(_publisher, domainEvents);
        }

        // последующее сохранение событий в http context
        await SaveChangesAsync();
    }

    private async Task PublishDomainEvents(IPublisher publisher, List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        // получаем очередь событий из http context
        var domainEventQueue = _httpContextAccessor.HttpContext!.Items.TryGetValue("DomainEventQueue", out var value)
                               && value is Queue<IDomainEvent> existingDomainEvent
            ? existingDomainEvent
            : new Queue<IDomainEvent>();

        // добавляем события домена в очередь
        domainEvents.ForEach(domainEventQueue.Enqueue);

        // сохраняем очередь в http context
        _httpContextAccessor.HttpContext!.Items["DomainEventQueue"] = domainEventQueue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;
}