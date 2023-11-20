using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedPartialSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleForceAggregation;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleSameTrackedEntriesTests;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;

public class IdentityResolutionTestsDbContext : DbContextBase
{
    public DbSet<RootNodeWithFirstTrackedCompositionCollection> RootNodesWithFirstTrackedCompositionCollections
    {
        get;
        set;
    }

    public DbSet<RootNodeWithFirstTrackedAggregationReference> RootNodesWithFirstTrackedAggregationReferences
    {
        get;
        set;
    }

    public DbSet<RootNodeWithFirstTrackedAggregationReferenceAndSubtree>
        RootNodeWithFirstTrackedAggregationReferencesAndSubtrees { get; set; }

    public DbSet<MultiForceAggregationRoot> MultiForceAggregationRoots { get; set; }

    public DbSet<AdvancedSubTreeRootNodeWithFirstTrackedComposition> AdvancedRootsWithFirstTrackedComposition
    {
        get;
        set;
    }

    public DbSet<AdvancedSubTreeRootNodeWithFirstTrackedAggregation> AdvancedRootsWithFirstTrackedAggregation
    {
        get;
        set;
    }

    public DbSet<RootNodeWithExtraLayerAfterAggregation> RootsWithExtraLayerAfterAggregations { get; set; }

    public DbSet<RootNodeWithReferenceNavigations> RootWithMultipleSameReferenceNavigationItems { get; set; }
    
    public DbSet<RootNodeWithCollectionNavigations> RootWithMultipleSameCollectionNavigationItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RootNodeWithFirstTrackedAggregationReferenceAndSubtree>()
            .Navigation(r => r.A_Item)
            .AutoInclude();

        modelBuilder.Entity<RootNodeWithFirstTrackedAggregationReferenceAndSubtree>()
            .Navigation(r => r.B_Item)
            .AutoInclude();

        modelBuilder.Entity<SubTreeRootNode>()
            .Navigation(r => r.ReferenceItem)
            .AutoInclude();

        modelBuilder.Entity<SubTreeRootNode>()
            .Navigation(r => r.SubTreeListItems)
            .AutoInclude();

        modelBuilder.Entity<SubTreeReferenceItem>()
            .Navigation(i => i.SubTreeChildItems)
            .AutoInclude();

        modelBuilder.Entity<MultiForceAggregationRoot>()
            .HasOne(rn => rn.CompositionItem)
            .WithMany()
            .IsRequired();

        modelBuilder.Entity<MultiForceAggregationRoot>()
            .HasOne(rn => rn.AggregationItem)
            .WithMany()
            .IsRequired(false);

        modelBuilder.Entity<CompositionItem>()
            .HasOne(c => c.AggregationItem)
            .WithMany()
            .IsRequired(false);

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.A_CompositionNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.B_AggregationNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedAggregation>()
            .Navigation(r => r.B_CompositionNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedAggregation>()
            .Navigation(r => r.A_AggregationNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.A_Composition)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.B_Aggregation)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.A_Compositions)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.B_Aggregations)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAggregation>()
            .Navigation(r => r.A_Aggregation)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAggregation>()
            .Navigation(r => r.B_Composition)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAggregation>()
            .Navigation(r => r.A_Aggregations)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAggregation>()
            .Navigation(r => r.B_Compositions)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeNode1>()
            .Navigation(n => n.CompositionNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeNode2>()
            .Navigation(n => n.CompositionNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeNode2>()
            .Navigation(n => n.CompositionListItems)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeNode3>()
            .Navigation(n => n.CompositionNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeNode3>()
            .Navigation(n => n.CompositionNode2)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeListItem1>()
            .Navigation(n => n.CompositionListItems)
            .AutoInclude();
    }
}