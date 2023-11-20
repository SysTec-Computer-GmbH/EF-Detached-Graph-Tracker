using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.ManyToMany;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.Nested;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.OneToMany;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Database;

public class BackreferenceTestDbContext : DbContextBase
{
    public DbSet<OneItem> OneItems { get; set; }

    public DbSet<ManyItemOne> ManyItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConcreteTypeWithAbstractTypeCollection>().HasBaseType<AbstractBaseTypeWithBackreference>();
    }
}