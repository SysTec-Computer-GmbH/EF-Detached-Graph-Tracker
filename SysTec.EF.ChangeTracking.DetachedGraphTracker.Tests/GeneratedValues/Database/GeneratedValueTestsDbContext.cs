using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.GeneratedValues.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.GeneratedValues.Database;

public class GeneratedValueTestsDbContext : DbContextBase
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntryWithGeneratedValue>()
            .Property(e => e.GeneratedValue)
            .UseIdentityAlwaysColumn()
            .HasIdentityOptions(10);
    }
}