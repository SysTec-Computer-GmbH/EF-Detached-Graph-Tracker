using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Database;

public class ImplementationShowcaseTestsDbContext : DbContextBase
{
    public DbSet<EntityWithPropertiesNewInitializer> EntitiesWithCompositionNew { get; set; }

    public DbSet<RootNode> RootNodes { get; set; }
}