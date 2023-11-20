using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class AdvancedSubtreeIdentityResolutionTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task _01_IdentityResolution_ForReferenceNavigation_WithFirstTrackedCompositionSubtree()
    {
        var root = new AdvancedSubTreeRootNodeWithFirstTrackedComposition
        {
            Text = "Root Init",
            A_CompositionNode = GetInitialSubTreeRoot()
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedComposition
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.A_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }

        var rootUpdate = (AdvancedSubTreeRootNodeWithFirstTrackedComposition)root.Clone();
        rootUpdate.B_AssociationNode = (AdvancedSubTreeNode1)root.A_CompositionNode!.Clone();
        rootUpdate.B_AssociationNode.CompositionNode.CompositionNode.Text = "Subtree Node 3 Updated";
        rootUpdate.B_AssociationNode.CompositionNode.CompositionListItems[0].Text = "Subtree List Item 1 Updated";
        rootUpdate.B_AssociationNode.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text =
            "Subtree List Item 2 Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedComposition
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.A_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem1)));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem2)));

            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);

            Assert.That(rootFromDb.B_AssociationNode, Is.Not.Null);
            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem1)));
            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.B_AssociationNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem2)));

            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode.CompositionNode.Text,
                Is.EqualTo(nameof(AdvancedSubTreeNode3)));
            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_AssociationNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }
    }

    [Test]
    public async Task _02_IdentityResolution_ForReferenceNavigation_WithFirstTrackedAssociationSubtree()
    {
        var root = new AdvancedSubTreeRootNodeWithFirstTrackedAssociation
        {
            Text = "Root Init",
            B_CompositionNode = GetInitialSubTreeRoot()
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedAssociation
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.B_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }

        var rootUpdate = (AdvancedSubTreeRootNodeWithFirstTrackedAssociation)root.Clone();
        rootUpdate.A_AssociationNode = (AdvancedSubTreeNode1)root.B_CompositionNode.Clone();

        rootUpdate.B_CompositionNode!.CompositionNode.CompositionNode.Text = "Subtree Node 3 Updated";
        rootUpdate.B_CompositionNode!.CompositionNode.CompositionListItems[0].Text = "Subtree List Item 1 Updated";
        rootUpdate.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text =
            "Subtree List Item 2 Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedAssociation
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.B_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 1 Updated"));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 2 Updated"));

            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.Text,
                Is.EqualTo("Subtree Node 3 Updated"));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);

            Assert.That(rootFromDb.A_AssociationNode, Is.Not.Null);
            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 1 Updated"));
            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.A_AssociationNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 2 Updated"));

            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode.CompositionNode.Text,
                Is.EqualTo("Subtree Node 3 Updated"));
            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_AssociationNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }
    }

    [Test]
    public async Task _03_IdentityResolution_ForCollectionNavigation_WithFirstTrackedCompositionSubtree()
    {
        var root = new AdvancedSubTreeRootNodeWithFirstTrackedComposition
        {
            Text = "Root Init",
            A_CompositionNode = GetInitialSubTreeRoot()
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedComposition
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.A_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }

        var rootUpdate = (AdvancedSubTreeRootNodeWithFirstTrackedComposition)root.Clone();
        rootUpdate.B_AssociationNodes.Add((AdvancedSubTreeNode1)root.A_CompositionNode.Clone());
        rootUpdate.B_AssociationNodes[0].CompositionNode.CompositionNode.Text = "Subtree Node 3 Updated";
        rootUpdate.B_AssociationNodes[0].CompositionNode.CompositionListItems[0].Text = "Subtree List Item 1 Updated";
        rootUpdate.B_AssociationNodes[0].CompositionNode.CompositionListItems[0].CompositionListItems[0].Text =
            "Subtree List Item 2 Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedComposition
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.A_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem1)));
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.A_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem2)));

            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);

            Assert.That(rootFromDb.B_AssociationNodes, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem1)));
            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo(nameof(AdvancedSubTreeListItem2)));

            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionNode.Text,
                Is.EqualTo(nameof(AdvancedSubTreeNode3)));
            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_AssociationNodes[0].CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }
    }

    [Test]
    public async Task _04_IdentityResolution_ForCollectionNavigation_WithFirstTrackedAssociationSubtree()
    {
        var root = new AdvancedSubTreeRootNodeWithFirstTrackedAssociation
        {
            Text = "Root Init",
            B_CompositionNode = GetInitialSubTreeRoot()
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedAssociation
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.B_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }

        var rootUpdate = (AdvancedSubTreeRootNodeWithFirstTrackedAssociation)root.Clone();
        rootUpdate.A_AssociationNodes.Add((AdvancedSubTreeNode1)root.B_CompositionNode.Clone());

        rootUpdate.B_CompositionNode!.CompositionNode.CompositionNode.Text = "Subtree Node 3 Updated";
        rootUpdate.B_CompositionNode!.CompositionNode.CompositionListItems[0].Text = "Subtree List Item 1 Updated";
        rootUpdate.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text =
            "Subtree List Item 2 Updated";

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await dbContext.AdvancedRootsWithFirstTrackedAssociation
                .SingleAsync(r => r.Id == root.Id);

            Assert.That(rootFromDb.B_CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 1 Updated"));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.B_CompositionNode!.CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 2 Updated"));

            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.Text,
                Is.EqualTo("Subtree Node 3 Updated"));
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.B_CompositionNode!.CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);

            Assert.That(rootFromDb.A_AssociationNodes, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode, Is.Not.Null);

            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionListItems, Has.Count.EqualTo(1));
            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 1 Updated"));
            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionListItems[0].CompositionListItems,
                Has.Count.EqualTo(1));
            Assert.That(
                rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionListItems[0].CompositionListItems[0].Text,
                Is.EqualTo("Subtree List Item 2 Updated"));

            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionNode.Text,
                Is.EqualTo("Subtree Node 3 Updated"));
            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionNode.CompositionNode, Is.Not.Null);
            Assert.That(rootFromDb.A_AssociationNodes[0].CompositionNode.CompositionNode.CompositionNode2, Is.Not.Null);
        }
    }

    [Test]
    public async Task
        _05_TrackingAnAssociationNode_NestedInSubtreeWithoutAssociationAttributeOverInboundNavigation_WithExistingCompositionInChangeTracker_DoesNotThrow()
    {
        var root = new RootNodeWithExtraLayerAfterAssociation
        {
            A_Composition = new TrackedItem
            {
                Text = "Composition Item"
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        var associationRoot = new AssociationRoot
        {
            Composition = new ExtraLayerItem
            {
                Composition = (TrackedItem)root.A_Composition.Clone()
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Update(associationRoot);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (RootNodeWithExtraLayerAfterAssociation)root.Clone();
        rootUpdate.B_Association = (AssociationRoot)associationRoot.Clone();

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () =>  await graphTracker.TrackGraphAsync(rootUpdate));
            await dbContext.SaveChangesAsync();
        }
    }

    private AdvancedSubTreeNode1 GetInitialSubTreeRoot()
    {
        return new()
        {
            Text = nameof(AdvancedSubTreeNode1),
            CompositionNode = new()
            {
                Text = nameof(AdvancedSubTreeNode2),
                CompositionNode = new AdvancedSubTreeNode3
                {
                    Text = nameof(AdvancedSubTreeNode3),
                    CompositionNode = new AdvancedSubTreeNode4
                    {
                        Text = nameof(AdvancedSubTreeNode4)
                    },
                    CompositionNode2 = new AdvancedSubTreeNode5
                    {
                        Text = nameof(AdvancedSubTreeNode5)
                    }
                },
                CompositionListItems = new()
                {
                    new AdvancedSubTreeListItem1
                    {
                        Text = nameof(AdvancedSubTreeListItem1),
                        CompositionListItems = new List<AdvancedSubTreeListItem2>
                        {
                            new()
                            {
                                Text = nameof(AdvancedSubTreeListItem2)
                            }
                        }
                    }
                }
            }
        };
    }
}