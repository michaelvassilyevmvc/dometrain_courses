using GymManagment.Application.Common.Interfaces;
using GymManagment.Infrastructure.Admins.Persistence;
using GymManagment.Infrastructure.Common.Persistence;
using GymManagment.Infrastructure.Gyms.Persistence;
using GymManagment.Infrastructure.Subscriptions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services.AddPersistence();
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<GymManagementDbContext>(options =>
        {
            options.UseSqlite("Data Source = GymManagement.db");
        });
        services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
        services.AddScoped<IAdminsRepository, AdminsRepository>();
        services.AddScoped<IGymsRepository, GymsRepository>();
        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<GymManagementDbContext>());
        return services;
    }
}