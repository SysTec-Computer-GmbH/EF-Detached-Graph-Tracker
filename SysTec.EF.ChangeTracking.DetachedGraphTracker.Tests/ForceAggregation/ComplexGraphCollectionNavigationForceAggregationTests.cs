using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.CollectionNavigation;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association;

public class ComplexGraphCollectionNavigationAssociationTests : TestBase<AssociationTestsDbContext>
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

        var rootNodeUpdate = (AssociationCollectionComplexRootNode)rootNode.Clone();
        rootNodeUpdate.Text = "RootNodeUpdate";
        rootNodeUpdate.SubTreeRoot.Text = "SubTreeRootUpdate";
        rootNodeUpdate.SubTreeRoot.ItemsL1[0].Text = "ItemL1Update";
        rootNodeUpdate.SubTreeRoot.ItemsL1[0].ItemsL2[0].Text = "ItemL2Update";

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationComplexCollectionComplexRootNodes
                .Include(x => x.SubTreeRoot)
                .ThenInclude(x => x.ItemsL1)
                .ThenInclude(x => x.ItemsL2)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Text, Is.EqualTo(rootNodeUpdate.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.Text, Is.EqualTo(rootNode.SubTreeRoot.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemsL1[0].Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemsL1[0].Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemsL1[0].ItemsL2[0].Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemsL1[0].ItemsL2[0].Text));
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

        var rootNodeUpdate = (AssociationCollectionComplexRootNode)rootNode.Clone();
        rootNodeUpdate.SubTreeRoot.ItemsL1[0].ItemsL2.Clear();

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.AssociationComplexCollectionComplexRootNodes
                .Include(x => x.SubTreeRoot)
                .ThenInclude(x => x.ItemsL1)
                .ThenInclude(x => x.ItemsL2)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.Text, Is.EqualTo(rootNodeUpdate.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.Text, Is.EqualTo(rootNode.SubTreeRoot.Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemsL1[0].Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemsL1[0].Text));
                Assert.That(rootNodeFromDb.SubTreeRoot.ItemsL1[0].ItemsL2[0].Text,
                    Is.EqualTo(rootNode.SubTreeRoot.ItemsL1[0].ItemsL2[0].Text));
            });
        }
    }

    [Test]
    public async Task _03_Relationships_InAssociationSubtree_ShouldNotBeConnected()
    {
        var itemL1 = new AssociationCollectionSubTreeItemL1
        {
            Text = "ItemL1"
        };

        var itemL2 = new AssociationSubTreeItemL2
        {
            Text = "ItemL2"
        };

        var subTreeRootItem = new AssociationCollectionSubTreeRoot
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
        var subTreeRootClone = (AssociationCollectionSubTreeRoot)subTreeRootItem.Clone();
        subTreeRootClone.ItemsL1.Add((AssociationCollectionSubTreeItemL1)itemL1.Clone());
        subTreeRootClone.ItemsL1[0].ItemsL2.Add((AssociationSubTreeItemL2)itemL2.Clone());
        var rootNode = new AssociationCollectionComplexRootNode
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
            var rootNodeFromDb = await dbContext.AssociationComplexCollectionComplexRootNodes
                .Include(r => r.SubTreeRoot)
                .ThenInclude(sn => sn.ItemsL1)
                .ThenInclude(sn => sn.ItemsL2)
                .SingleAsync(r => r.Id == rootNode.Id);

            Assert.That(rootNodeFromDb.SubTreeRoot, Is.Not.Null);
            Assert.That(rootNodeFromDb.SubTreeRoot.ItemsL1, Is.Empty);
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var itemL1FromDb = await dbContext.Set<AssociationCollectionSubTreeItemL1>()
                .Include(i => i.ItemsL2)
                .SingleAsync(i => i.Id == itemL1.Id);

            Assert.That(itemL1FromDb.ItemsL2, Is.Empty);
        }
    }


    private AssociationCollectionComplexRootNode GetRootNode()
    {
        return new AssociationCollectionComplexRootNode
        {
            Text = "RootNode",
            SubTreeRoot = new AssociationCollectionSubTreeRoot
            {
                Text = "SubTreeRoot",
                ItemsL1 = new List<AssociationCollectionSubTreeItemL1>
                {
                    new()
                    {
                        Text = "ItemL1_1",
                        ItemsL2 = new List<AssociationSubTreeItemL2>
                        {
                            new()
                            {
                                Text = "ItemL2_1"
                            }
                        }
                    }
                }
            }
        };
    }
}