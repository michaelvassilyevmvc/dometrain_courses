using DomeGym.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DomeGym.Infrastructure.Middleware;

public class EventualConsistencyMiddleware
{
    public const string DomainEventsKey = "DomainEventsKey";
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, DomeGymDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(key: DomainEventsKey, value: out var value) &&
                    value is Queue<IDomainEvent> domainEvents)
                {
                    while (domainEvents.TryDequeue(result: out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (EventualConsistencyException e)
            {
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });
        await _next(context);
    }
}