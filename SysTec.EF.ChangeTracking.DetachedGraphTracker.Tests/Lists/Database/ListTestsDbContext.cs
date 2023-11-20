using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Inheritance;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;

public class ListTestsDbContext : DbContextBase
{
    public DbSet<RootNodeWithRequiredSimpleList> RootNodesWithRequiredSimpleList { get; set; }

    public DbSet<RootNodeWithOptionalSimpleList> RootNodesWithOptionalSimpleList { get; set; }

    public DbSet<ConcreteTypeWithConcreteCollection> ConcreteTypes { get; set; }
    public DbSet<RootNodeWithOptionalSimpleListAndForceDelete> RootNodesWithOptionalSimpleListAndForceDelete
    {
        get;
        set;
    }

    public DbSet<RootNodeWithOptionalSimpleListAndForceAggregation> RootNodesWithOptionalSimpleListAndForceAggregation
    {
        get;
        set;
    }

    public DbSet<RootNodeWithMultipleCompositionsOfSameType> RootNodesWithMultipleCompositionsOfSameType { get; set; }

    public DbSet<RootNodeWithMultipleNestedCompositionsOfSameType> RootNodesWithMultipleNestedCompositionsOfSameType
    {
        get;
        set;
    }

    public DbSet<RootNodeWithCompositionAndAggregationOfSameType> RootNodesWithCompositionAndAggregationOfSameType
    {
        get;
        set;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RootNodeWithRequiredSimpleList>()
            .HasMany(rn => rn.ListItems)
            .WithOne()
            .IsRequired();
    }
}