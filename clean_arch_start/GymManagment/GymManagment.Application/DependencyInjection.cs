using GymManagment.Application.Gyms.Commands.CreateGym;
using GymManagment.Domain.Gyms;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ErrorOr;

namespace GymManagment.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            options.AddBehavior<IPipelineBehavior<CreateGymCommand, ErrorOr<Gym>>, CreateGymCommandBehavior>();
        });
        return services;
    }
}