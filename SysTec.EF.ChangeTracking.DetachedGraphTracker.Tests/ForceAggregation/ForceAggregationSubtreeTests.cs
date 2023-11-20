using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Subtree;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation;

public class ForceAggregationSubtreeTests : TestBase<ForceAggregationTestsDbContext>
{
    [Test]
    public async Task _01_RelationshipInsideExistingAggregation_WithinReferenceNavigation_CanNotBeModified()
    {
        var root = new RootNode
        {
            Aggregation = new AggregationRoot
            {
                Text = "Aggregation"
            }
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (RootNode)root.Clone();
        rootUpdate.Aggregation!.Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Aggregation)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Aggregation!.Composition, Is.Null); });
        }

        var aggregationUpdate = (AggregationRoot)root.Aggregation.Clone();
        aggregationUpdate.Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(aggregationUpdate);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate2 = (RootNode)root.Clone();
        rootUpdate2.Aggregation = (AggregationRoot)aggregationUpdate.Clone();
        rootUpdate2.Aggregation.Composition = null;

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Aggregation)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Aggregation!.Composition, Is.Not.Null); });
        }
    }

    [Test]
    public async Task _02_RelationshipInsideExistingAggregation_WithinCollectionNavigation_CanNotBeModified()
    {
        var root = new RootNode
        {
            Aggregations = new List<AggregationRoot>
            {
                new AggregationRoot
                {
                    Text = "Aggregation"
                }
            }
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (RootNode)root.Clone();
        rootUpdate.Aggregations[0].Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Aggregations)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Aggregations[0].Composition, Is.Null); });
        }

        var aggregationUpdate = (AggregationRoot)root.Aggregations[0].Clone();
        aggregationUpdate.Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(aggregationUpdate);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate2 = (RootNode)root.Clone();
        rootUpdate2.Aggregations.Clear();
        rootUpdate2.Aggregations.Add((AggregationRoot)aggregationUpdate.Clone());
        rootUpdate2.Aggregations[0].Composition = null;

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceAggregationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Aggregations)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Aggregations[0].Composition, Is.Not.Null); });
        }
    }
}