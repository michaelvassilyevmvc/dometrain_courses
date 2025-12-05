using System.Reflection;
using GymManagment.Application.Common.Interfaces;
using GymManagment.Domain.Admins;
using GymManagment.Domain.Gyms;
using GymManagment.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Infrastructure.Common.Persistence;

public class GymManagementDbContext : DbContext, IUnitOfWork
{
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Gym> Gyms { get; set; } = null!;

    public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options) : base(options)
    {
    }

    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}