using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.ReferenceNavigation;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation;

public class ComplexGraphReferenceNavigationForceAggregationTests : TestBase<ForceAggregationTestsDbContext>
{
    [Test]
    public async Task _01_Changes_InForceAggregationSubtree_ShouldNotBePersisted()
    {
        var rootNode = GetRootNode();

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (ForceAggregationReferenceComplexRootNode)rootNode.Clone();
        rootNodeUpdate.Text = "RootNodeUpdate";
        rootNodeUpdate.SubTreeRoot.Text = "SubTreeRootUpdate";
        rootNodeUpdate.SubTreeRoot.ItemL1.Text = "ItemL1Update";
        rootNodeUpdate.SubTreeRoot.ItemL1.ItemL2.Text = "ItemL2Update";

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.ForceAggregationComplexReferenceRootNodes
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
    public async Task _02_Relationships_InForceAggregationSubtree_ShouldNotBeDissolved()
    {
        var rootNode = GetRootNode();

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (ForceAggregationReferenceComplexRootNode)rootNode.Clone();
        rootNodeUpdate.SubTreeRoot.ItemL1.ItemL2 = null;

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.ForceAggregationComplexReferenceRootNodes
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
    public async Task _03_Relationships_InForceAggregationSubtree_ShouldNotBeConnected()
    {
        var itemL1 = new ForceAggregationReferenceSubTreeItemL1
        {
            Text = "ItemL1"
        };

        var itemL2 = new ForceAggregationSubTreeItemL2
        {
            Text = "ItemL2"
        };

        var subTreeRootItem = new ForceAggregationReferenceSubTreeRoot
        {
            Text = "SubTreeRoot"
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            dbContext.Add(itemL1);
            dbContext.Add(itemL2);
            dbContext.Add(subTreeRootItem);
            await dbContext.SaveChangesAsync();
        }

        // Clone to have another instance and to simulate a remote scenario
        var subTreeRootClone = (ForceAggregationReferenceSubTreeRoot)subTreeRootItem.Clone();
        subTreeRootClone.ItemL1 = (ForceAggregationReferenceSubTreeItemL1)itemL1.Clone();
        subTreeRootClone.ItemL1.ItemL2 = (ForceAggregationSubTreeItemL2)itemL2.Clone();
        var rootNode = new ForceAggregationReferenceComplexRootNode
        {
            Text = "RootNode",
            SubTreeRoot = subTreeRootClone
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.ForceAggregationComplexReferenceRootNodes
                .Include(r => r.SubTreeRoot)
                .ThenInclude(sn => sn.ItemL1)
                .ThenInclude(sn => sn.ItemL2)
                .SingleAsync(r => r.Id == rootNode.Id);

            Assert.That(rootNodeFromDb.SubTreeRoot, Is.Not.Null);
            Assert.That(rootNodeFromDb.SubTreeRoot.ItemL1, Is.Null);
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var itemL1FromDb = await dbContext.Set<ForceAggregationReferenceSubTreeItemL1>()
                .Include(i => i.ItemL2)
                .SingleAsync(i => i.Id == itemL1.Id);

            Assert.That(itemL1FromDb.ItemL2, Is.Null);
        }
    }


    private ForceAggregationReferenceComplexRootNode GetRootNode()
    {
        return new ForceAggregationReferenceComplexRootNode
        {
            Text = "RootNode",
            SubTreeRoot = new ForceAggregationReferenceSubTreeRoot
            {
                Text = "SubTreeRoot",
                ItemL1 = new ForceAggregationReferenceSubTreeItemL1
                {
                    Text = "ItemL1",
                    ItemL2 = new ForceAggregationSubTreeItemL2
                    {
                        Text = "ItemL2"
                    }
                }
            }
        };
    }
}