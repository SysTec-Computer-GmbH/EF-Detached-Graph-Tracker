using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedPartialSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleAssociation;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleSameTrackedEntriesTests;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;

public class IdentityResolutionTestsDbContext : DbContextBase
{
    public DbSet<RootNodeWithFirstTrackedCompositionCollection> RootNodesWithFirstTrackedCompositionCollections
    {
        get;
        set;
    }

    public DbSet<RootNodeWithFirstTrackedAssociationReference> RootNodesWithFirstTrackedAssociationReferences
    {
        get;
        set;
    }

    public DbSet<RootNodeWithFirstTrackedAssociationReferenceAndSubtree>
        RootNodeWithFirstTrackedAssociationReferencesAndSubtrees { get; set; }

    public DbSet<MultiAssociationRoot> MultiAssociationRoots { get; set; }

    public DbSet<AdvancedSubTreeRootNodeWithFirstTrackedComposition> AdvancedRootsWithFirstTrackedComposition
    {
        get;
        set;
    }

    public DbSet<AdvancedSubTreeRootNodeWithFirstTrackedAssociation> AdvancedRootsWithFirstTrackedAssociation
    {
        get;
        set;
    }

    public DbSet<RootNodeWithExtraLayerAfterAssociation> RootsWithExtraLayerAfterAssociations { get; set; }

    public DbSet<RootNodeWithReferenceNavigations> RootWithMultipleSameReferenceNavigationItems { get; set; }
    
    public DbSet<RootNodeWithCollectionNavigations> RootWithMultipleSameCollectionNavigationItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RootNodeWithFirstTrackedAssociationReferenceAndSubtree>()
            .Navigation(r => r.A_Item)
            .AutoInclude();

        modelBuilder.Entity<RootNodeWithFirstTrackedAssociationReferenceAndSubtree>()
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

        modelBuilder.Entity<MultiAssociationRoot>()
            .HasOne(rn => rn.CompositionItem)
            .WithMany()
            .IsRequired();

        modelBuilder.Entity<MultiAssociationRoot>()
            .HasOne(rn => rn.AssociationItem)
            .WithMany()
            .IsRequired(false);

        modelBuilder.Entity<CompositionItem>()
            .HasOne(c => c.AssociationItem)
            .WithMany()
            .IsRequired(false);

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.A_CompositionNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.B_AssociationNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedAssociation>()
            .Navigation(r => r.B_CompositionNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedSubTreeRootNodeWithFirstTrackedAssociation>()
            .Navigation(r => r.A_AssociationNode)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.A_Composition)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.B_Association)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.A_Compositions)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition>()
            .Navigation(r => r.B_Associations)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
            .Navigation(r => r.A_Association)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
            .Navigation(r => r.B_Composition)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
            .Navigation(r => r.A_Associations)
            .AutoInclude();

        modelBuilder.Entity<AdvancedPartialSubTreeRootNodeWithFirstTrackedAssociation>()
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