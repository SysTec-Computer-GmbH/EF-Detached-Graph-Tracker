using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism;

public class ReferenceNavigationTests : TestBase<PolymorphismTestsDbContext>
{
    [Test]
    public async Task _01_RelationshipsInCompositionReferenceNavigation_WithDifferentSubTypes_CanBeConnectedAndSevered()
    {
        var rootEntity = new RootEntityWithBaseTypeNavigations
        {
            Text = "Root Entity",
            CompositionItem = new SubEntityWithNormalKey
            {
                Text = "SubEntity"
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
                .Include(x => x.CompositionItem)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.CompositionItem, Is.Not.Null);
            Assert.That(rootEntityFromDb.CompositionItem, Is.TypeOf(typeof(SubEntityWithNormalKey)));
        }

        var rootEntityUpdate = (RootEntityWithBaseTypeNavigations)rootEntity.Clone();
        rootEntityUpdate.CompositionItem = new DifferentSubEntityWithNormalKey
        {
            Text = "Different SubEntity"
        };

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.CompositionItem)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.CompositionItem, Is.Not.Null);
            Assert.That(rootEntityFromDb.CompositionItem, Is.TypeOf(typeof(DifferentSubEntityWithNormalKey)));
        }

        var rootEntityUpdate2 = (RootEntityWithBaseTypeNavigations)rootEntityUpdate.Clone();
        rootEntityUpdate2.CompositionItem = null;

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.CompositionItem)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.CompositionItem, Is.Null);
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var baseTypesFromDb = await dbContext.Set<BaseEntity>().ToListAsync();

            Assert.Multiple(() =>
            {
                Assert.That(baseTypesFromDb, Has.Count.EqualTo(2));
                Assert.That(baseTypesFromDb.OfType<SubEntityWithNormalKey>().Count(), Is.EqualTo(1));
                Assert.That(baseTypesFromDb.OfType<DifferentSubEntityWithNormalKey>().Count(), Is.EqualTo(1));
            });
        }
    }

    [Test]
    public async Task _02_RelationshipsInAggregationReferenceNavigation_WithDifferentSubTypes_CanBeConnectedAndSevered()
    {
        var aggregationItem = new SubEntityWithNormalKey
        {
            Text = "SubEntity"
        };

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            dbContext.Add(aggregationItem);
            await dbContext.SaveChangesAsync();
        }

        var rootEntity = new RootEntityWithBaseTypeNavigations
        {
            Text = "Root Entity",
            AggregationItem = aggregationItem
        };

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.AggregationItem)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.AggregationItem, Is.Not.Null);
            Assert.That(rootEntityFromDb.AggregationItem, Is.TypeOf(typeof(SubEntityWithNormalKey)));
        }

        var differentAggregationItem = new DifferentSubEntityWithNormalKey
        {
            Text = "Different SubEntity"
        };

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            dbContext.Add(differentAggregationItem);
            await dbContext.SaveChangesAsync();
        }

        var rootEntityUpdate = (RootEntityWithBaseTypeNavigations)rootEntity.Clone();
        rootEntityUpdate.AggregationItem = (DifferentSubEntityWithNormalKey)differentAggregationItem.Clone();

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.AggregationItem)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.AggregationItem, Is.Not.Null);
            Assert.That(rootEntityFromDb.AggregationItem, Is.TypeOf(typeof(DifferentSubEntityWithNormalKey)));
        }

        var rootEntityUpdate2 = (RootEntityWithBaseTypeNavigations)rootEntityUpdate.Clone();
        rootEntityUpdate2.AggregationItem = null;

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootEntityUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var rootEntityFromDb = await dbContext.RootEntities
                .Include(x => x.AggregationItem)
                .SingleAsync(x => x.Id == rootEntity.Id);

            Assert.That(rootEntityFromDb.AggregationItem, Is.Null);
        }

        await using (var dbContext = new PolymorphismTestsDbContext())
        {
            var baseTypesFromDb = await dbContext.Set<BaseEntity>().ToListAsync();

            Assert.Multiple(() =>
            {
                Assert.That(baseTypesFromDb, Has.Count.EqualTo(2));
                Assert.That(baseTypesFromDb.OfType<SubEntityWithNormalKey>().Count(), Is.EqualTo(1));
                Assert.That(baseTypesFromDb.OfType<DifferentSubEntityWithNormalKey>().Count(), Is.EqualTo(1));
            });
        }
    }
}