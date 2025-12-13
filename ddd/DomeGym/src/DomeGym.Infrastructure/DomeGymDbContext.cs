using DomeGym.Domain.AdminAggregate;
using DomeGym.Domain.Common;
using DomeGym.Domain.GymAggregate;
using DomeGym.Domain.ParticipantAggregate;
using DomeGym.Domain.RoomAggregate;
using DomeGym.Domain.SessionAggregate;
using DomeGym.Domain.SubscriptionAggregate;
using DomeGym.Domain.TrainerAggregate;
using DomeGym.Infrastructure.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DomeGym.Infrastructure;

public class DomeGymDbContext: DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Gym> Gyms { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<Participant> Participants { get; set; } = null!;
    public DbSet<Trainer> Trainers { get; set; } = null!;
    public DbSet<Admin> Admins { get; set; } = null!;

    public DomeGymDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) 
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomeGymDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<AggregateRoot>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(domainEvents => domainEvents)
            .ToList();
        
        var result = await base.SaveChangesAsync(cancellationToken);

        Queue<IDomainEvent> domainEventsQueue = _httpContextAccessor.HttpContext.Items.TryGetValue(
            key: EventualConsistencyMiddleware.DomainEventsKey,
            value: out var value) && value is Queue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;

        return result;
    }
}