using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Behavior;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.CollectionNavigation;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.ReferenceNavigation;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Subtree;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Database;

public class ForceAggregationTestsDbContext : DbContextBase
{
    public DbSet<ForceAggregationRootNode> ForceAggregationRootNodes { get; set; }

    public DbSet<ForceAggregationReferenceComplexRootNode> ForceAggregationComplexReferenceRootNodes { get; set; }

    public DbSet<RootWithDetachBehavior> RootsWithDetachBehaviors { get; set; }
    
    public DbSet<RootWithDefaultThrowBehavior> RootsWithDefaultThrowBehaviors { get; set; }

    public DbSet<ForceAggregationCollectionComplexRootNode> ForceAggregationComplexCollectionComplexRootNodes
    {
        get;
        set;
    }

    public DbSet<RootNode> RootNodes { get; set; }
}