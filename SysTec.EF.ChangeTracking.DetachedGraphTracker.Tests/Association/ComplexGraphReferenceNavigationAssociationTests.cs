using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.ReferenceNavigation;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association;

public class ComplexGraphReferenceNavigationAssociationTests : TestBase<AssociationTestsDbContext>
{
    [Test]
    public async Task _01_Changes_InAssociationSubtree_ShouldNotBePersisted()
    {
        var rootNode = GetRootNode();

        await using (var dbContext = new AssociationTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (AssociationReferenceComplexRootNode)rootNode.Clone();
        rootNodeUpdate.Text = "RootNodeUpdate";
        rootNodeUpdate.SubTreeRoot.Text = "SubTreeRootUpdate";
        rootNodeUpdate.SubTreeRoot.ItemL1.Text = "ItemL1Update";
        rootNodeUpdate.SubTreeRoot.ItemL1.ItemL2.Text = "ItemL2Update";

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationComplexReferenceRootNodes
                .Include(x => x.SubTreeRoot)
                .ThenInclude(x => x.ItemL1)
                .ThenInclude(x => x.ItemL2)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Text, Is.EqualTo(rootNodeUpdate.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.Text, Is.EqualTo(rootNode.SubTreeRoot.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemL1.Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemL1.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemL1.ItemL2.Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemL1.ItemL2.Text));
            });
        }
    }

    [Test]
    public async Task _02_Relationships_InAssociationSubtree_ShouldNotBeDissolved()
    {
        var rootNode = GetRootNode();

        await using (var dbContext = new AssociationTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (AssociationReferenceComplexRootNode)rootNode.Clone();
        rootNodeUpdate.SubTreeRoot.ItemL1.ItemL2 = null;

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationComplexReferenceRootNodes
                .Include(x => x.SubTreeRoot)
                .ThenInclude(x => x.ItemL1)
                .ThenInclude(x => x.ItemL2)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Text, Is.EqualTo(rootNodeUpdate.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.Text, Is.EqualTo(rootNode.SubTreeRoot.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemL1.Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemL1.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemL1.ItemL2.Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemL1.ItemL2.Text));
            });
        }
    }

    [Test]
    public async Task _03_Relationships_InAssociationSubtree_ShouldNotBeConnected()
    {
        var itemL1 = new AssociationReferenceSubTreeItemL1
        {
            Text = "ItemL1"
        };

        var itemL2 = new AssociationSubTreeItemL2
        {
            Text = "ItemL2"
        };

        var subTreeRootItem = new AssociationReferenceSubTreeRoot
        {
            Text = "SubTreeRoot"
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            dbContext.Add(itemL1);
            dbContext.Add(itemL2);
            dbContext.Add(subTreeRootItem);
            await dbContext.SaveChangesAsync();
        }

        // Clone to have another instance and to simulate a remote scenario
        var subTreeRootClone = (AssociationReferenceSubTreeRoot)subTreeRootItem.Clone();
        subTreeRootClone.ItemL1 = (AssociationReferenceSubTreeItemL1)itemL1.Clone();
        subTreeRootClone.ItemL1.ItemL2 = (AssociationSubTreeItemL2)itemL2.Clone();
        var rootNode = new AssociationReferenceComplexRootNode
        {
            Text = "RootNode",
            SubTreeRoot = subTreeRootClone
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationComplexReferenceRootNodes
                .Include(r => r.SubTreeRoot)
                .ThenInclude(sn => sn.ItemL1)
                .ThenInclude(sn => sn.ItemL2)
                .SingleAsync(r => r.Id == rootNode.Id);

            Assert.That(rootNodeFromDb.SubTreeRoot, Is.Not.Null);
            Assert.That(rootNodeFromDb.SubTreeRoot.ItemL1, Is.Null);
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var itemL1FromDb = await dbContext.Set<AssociationReferenceSubTreeItemL1>()
                .Include(i => i.ItemL2)
                .SingleAsync(i => i.Id == itemL1.Id);

            Assert.That(itemL1FromDb.ItemL2, Is.Null);
        }
    }


    private AssociationReferenceComplexRootNode GetRootNode()
    {
        return new AssociationReferenceComplexRootNode
        {
            Text = "RootNode",
            SubTreeRoot = new AssociationReferenceSubTreeRoot
            {
                Text = "SubTreeRoot",
                ItemL1 = new AssociationReferenceSubTreeItemL1
                {
                    Text = "ItemL1",
                    ItemL2 = new AssociationSubTreeItemL2
                    {
                        Text = "ItemL2"
                    }
                }
            }
        };
    }
}