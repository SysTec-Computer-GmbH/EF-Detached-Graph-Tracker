using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleForceAggregation;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class MultipleReferenceForceAggregationIdentityResolutionTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task
        _01_IdentityResolution_ForMultipleReferenceForceAggregationsInGraph_ShouldNotAffectAggregationBehavior()
    {
        var forceAggregationItem = await CreateForeAggregationItem();

        var aggregationClone1 = (ForceAggregationItem)forceAggregationItem.Clone();
        aggregationClone1.Text = "1ShouldNotBePersisted";

        var aggregationClone2 = (ForceAggregationItem)forceAggregationItem.Clone();
        aggregationClone2.Text = "2ShouldNotBePersisted";

        var root = new MultiForceAggregationRoot
        {
            Text = "AggregationRoot",
            AggregationItem = aggregationClone1,
            CompositionItem = new CompositionItem
            {
                Text = "CompositionItem",
                AggregationItem = aggregationClone2
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        MultiForceAggregationRoot clonedRootFromDb;
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await GetForceAggregationRootFromDb(dbContext, root.Id);

            clonedRootFromDb = (MultiForceAggregationRoot)rootFromDb.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.AggregationItem, Is.Not.Null);
                Assert.That(rootFromDb.AggregationItem!.Id, Is.EqualTo(forceAggregationItem.Id));
                Assert.That(rootFromDb.AggregationItem.Text, Is.EqualTo(forceAggregationItem.Text));
            });

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.CompositionItem.AggregationItem.Id, Is.EqualTo(forceAggregationItem.Id));
                Assert.That(rootFromDb.CompositionItem.AggregationItem.Text, Is.EqualTo(forceAggregationItem.Text));
            });
        }

        clonedRootFromDb.CompositionItem.AggregationItem = null;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(clonedRootFromDb);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeFromDb = await GetForceAggregationRootFromDb(dbContext, clonedRootFromDb.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.AggregationItem, Is.Not.Null);
                Assert.That(rootNodeFromDb.AggregationItem!.Id, Is.EqualTo(forceAggregationItem.Id));
                Assert.That(rootNodeFromDb.AggregationItem.Text, Is.EqualTo(forceAggregationItem.Text));
            });

            Assert.That(rootNodeFromDb.CompositionItem.AggregationItem, Is.Null);
        }
    }

    [Test]
    public async Task
        _02_IdentityResolution_ForMultipleCollectionForceAggregationsInGraph_ShouldNotAffectAggregationBehavior()
    {
        var forceAggregationItem = await CreateForeAggregationItem();

        var aggregationClone1 = (ForceAggregationItem)forceAggregationItem.Clone();
        aggregationClone1.Text = "1ShouldNotBePersisted";

        var aggregationClone2 = (ForceAggregationItem)forceAggregationItem.Clone();
        aggregationClone2.Text = "2ShouldNotBePersisted";

        var root = new MultiForceAggregationRoot
        {
            Text = "AggregationRoot",
            AggregationItems = { aggregationClone1 },
            CompositionItem = new CompositionItem
            {
                Text = "CompositionItem",
                AggregationItems = { aggregationClone2 }
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        MultiForceAggregationRoot clonedRootFromDb;
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await GetForceAggregationRootFromDb(dbContext, root.Id);

            clonedRootFromDb = (MultiForceAggregationRoot)rootFromDb.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.AggregationItems, Has.Count.EqualTo(1));
                Assert.That(rootFromDb.AggregationItems[0].Id, Is.EqualTo(forceAggregationItem.Id));
                Assert.That(rootFromDb.AggregationItems[0].Text, Is.EqualTo(forceAggregationItem.Text));
            });

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.CompositionItem.AggregationItems, Has.Count.EqualTo(1));
                Assert.That(rootFromDb.CompositionItem.AggregationItems[0].Id, Is.EqualTo(forceAggregationItem.Id));
                Assert.That(rootFromDb.CompositionItem.AggregationItems[0].Text, Is.EqualTo(forceAggregationItem.Text));
            });
        }

        clonedRootFromDb.CompositionItem.AggregationItems.Clear();

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(clonedRootFromDb);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeFromDb = await GetForceAggregationRootFromDb(dbContext, clonedRootFromDb.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.AggregationItems, Has.Count.EqualTo(1));
                Assert.That(rootNodeFromDb.AggregationItems[0].Id, Is.EqualTo(forceAggregationItem.Id));
                Assert.That(rootNodeFromDb.AggregationItems[0].Text, Is.EqualTo(forceAggregationItem.Text));
            });

            Assert.That(rootNodeFromDb.CompositionItem.AggregationItems, Is.Empty);
        }
    }

    private async Task<ForceAggregationItem> CreateForeAggregationItem()
    {
        var forceAggregationItem = new ForceAggregationItem
        {
            Text = "ForceAggregationItem"
        };

        await using var dbContext = new IdentityResolutionTestsDbContext();
        dbContext.Add(forceAggregationItem);
        await dbContext.SaveChangesAsync();

        return forceAggregationItem;
    }

    private async Task<MultiForceAggregationRoot> GetForceAggregationRootFromDb(
        IdentityResolutionTestsDbContext dbContext, int id)
    {
        return await dbContext.MultiForceAggregationRoots
            .Include(rn => rn.AggregationItems)
            .Include(rn => rn.AggregationItem)
            .Include(rn => rn.CompositionItem.AggregationItem)
            .Include(rn => rn.CompositionItem.AggregationItems)
            .SingleAsync(rn => rn.Id == id);
    }
}