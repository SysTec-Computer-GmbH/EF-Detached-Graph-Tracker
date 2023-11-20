using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism;

public class CollectionNavigationTests : TestBase<PolymorphismTestsDbContext>
{
    [Test]
    public async Task
        _01_RelationshipsInCompositionCollectionNavigation_WithDifferentSubTypes_CanBeConnectedAndSevered()
    {
        var rootEntity = new RootEntityWithBaseTypeNavigations
        {
            Text = "Root Entity",
            CompositionItems = new List<BaseEntity>
            {
                new SubEntityWithNormalKey
                {
                    Text = "SubEntity"
                },
                new DifferentSubEntityWithNormalKey
                {
                    Text = "Different SubEntity"
                }
            }
        };

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.CompositionItems)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.CompositionItems, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(rootEntityFromDb.CompositionItems.OfType<SubEntityWithNormalKey>().Count(), Is.EqualTo(1));
                Assert.That(rootEntityFromDb.CompositionItems.OfType<DifferentSubEntityWithNormalKey>().Count(),
                    Is.EqualTo(1));
            });
        }

        var rootEntityUpdate = (RootEntityWithBaseTypeNavigations)rootEntity.Clone();
        rootEntityUpdate.CompositionItems.RemoveAt(0);

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.CompositionItems)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.CompositionItems, Has.Count.EqualTo(1));
            Assert.That(rootEntityFromDb.CompositionItems[0], Is.TypeOf(typeof(DifferentSubEntityWithNormalKey)));
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var subEntities = await dbContext.Set<BaseEntity>().ToListAsync();
            Assert.That(subEntities, Has.Count.EqualTo(2));
        }

        var rootEntityUpdate2 = (RootEntityWithBaseTypeNavigations)rootEntityUpdate.Clone();
        rootEntityUpdate2.CompositionItems = null;

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.CompositionItems)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.CompositionItems, Is.Empty);
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var baseTypesFromDb = await dbContext.Set<BaseEntity>().ToListAsync();
            Assert.That(baseTypesFromDb, Has.Count.EqualTo(2));
        }
    }

    [Test]
    public async Task
        _02_RelationshipsInAggregationCollectionNavigation_WithDifferentSubTypes_CanBeConnectedAndSevered()
    {
        var aggregationItem1 = new SubEntityWithNormalKey
        {
            Text = "SubEntity 1"
        };

        var aggregationItem2 = new DifferentSubEntityWithNormalKey
        {
            Text = "SubEntity 2"
        };

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            dbContext.Add(aggregationItem1);
            dbContext.Add(aggregationItem2);
            await dbContext.SaveChangesAsync();
        }

        var rootEntity = new RootEntityWithBaseTypeNavigations
        {
            Text = "Root Entity",
            AggregationItems =
            {
                (SubEntityWithNormalKey)aggregationItem1.Clone(),
                (DifferentSubEntityWithNormalKey)aggregationItem2.Clone()
            }
        };

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.AggregationItems)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.AggregationItems, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(rootEntityFromDb.AggregationItems.OfType<SubEntityWithNormalKey>().Count(), Is.EqualTo(1));
                Assert.That(rootEntityFromDb.AggregationItems.OfType<DifferentSubEntityWithNormalKey>().Count(),
                    Is.EqualTo(1));
            });
        }

        var rootEntityUpdate = (RootEntityWithBaseTypeNavigations)rootEntity.Clone();
        rootEntityUpdate.AggregationItems.RemoveAt(0);

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.AggregationItems)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.AggregationItems, Has.Count.EqualTo(1));
            Assert.That(rootEntityFromDb.AggregationItems[0], Is.TypeOf(typeof(DifferentSubEntityWithNormalKey)));
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var subEntities = await dbContext.Set<BaseEntity>().ToListAsync();
            Assert.That(subEntities, Has.Count.EqualTo(2));
        }

        var rootEntityUpdate2 = (RootEntityWithBaseTypeNavigations)rootEntityUpdate.Clone();
        rootEntityUpdate2.AggregationItems = null;

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.AggregationItems)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.AggregationItems, Is.Empty);
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var baseTypesFromDb = await dbContext.Set<BaseEntity>().ToListAsync();
            Assert.That(baseTypesFromDb, Has.Count.EqualTo(2));
        }
    }
}