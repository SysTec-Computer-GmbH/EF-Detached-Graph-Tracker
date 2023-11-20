using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Behavior;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.CollectionNavigation;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.ReferenceNavigation;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Subtree;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Database;

public class AssociationTestsDbContext : DbContextBase
{
    public DbSet<AssociationRootNode> AssociationRootNodes { get; set; }

    public DbSet<AssociationReferenceComplexRootNode> AssociationComplexReferenceRootNodes { get; set; }

    public DbSet<RootWithDetachBehavior> RootsWithDetachBehaviors { get; set; }
    
    public DbSet<RootWithDefaultThrowBehavior> RootsWithDefaultThrowBehaviors { get; set; }

    public DbSet<AssociationCollectionComplexRootNode> AssociationComplexCollectionComplexRootNodes
    {
        get;
        set;
    }

    public DbSet<RootNode> RootNodes { get; set; }
}