using GymManagment.Infrastructure.Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GymManagment.Infrastructure;

public static class RequestPipeline
{
    public static IApplicationBuilder AddInfrasturctureMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<EventualConsistencyMiddleware>();
        return builder;
    }
}