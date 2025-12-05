using GymManagment.Domain.Common;
using GymManagment.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManagment.Infrastructure.Common.Middleware;

public class EventualConsistencyMiddleware
{
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, GymManagementDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue("DomainEventQueue", out var value)
                    && value is Queue<IDomainEvent> domainEventsQueue)
                {
                    while (domainEventsQueue!.TryDequeue(out var domainEvent))
                    {
                        await publisher.Publish(domainEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                // предупредить клиента что несмотря на ответ 204 
                // изменения не произошли из-за непредвиденной ошибки
               
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });
        await _next(context);
    }
}