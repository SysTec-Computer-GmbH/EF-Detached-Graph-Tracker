using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.BugReports;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Database;

public class EfDefaultBugReportsDbContext : DbContextBase
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseType>()
            .UseTphMappingStrategy()
            .HasDiscriminator(d => d.Discriminator)
            .HasValue<ConcreteTypeOne>(nameof(ConcreteTypeOne))
            .HasValue<ConcreteTypeTwo>(nameof(ConcreteTypeTwo));
    }
}