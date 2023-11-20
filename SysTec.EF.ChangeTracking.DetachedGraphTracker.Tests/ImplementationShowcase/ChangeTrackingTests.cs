using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase;

public class ChangeTrackingTests : TestBase<ImplementationShowcaseTestsDbContext>
{
    [Test]
    public async Task _01_ShowcaseTrackingBehaviorBeforeBusinessLogic_ForCompositions()
    {
        var entity = new RootNode
        {
            Composition = new Entity
            {
                Text = "Composition Init"
            }
        };

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(n => n.Composition)
                .SingleAsync();

            Assert.That(rootFromDb, Is.Not.Null);
            Assert.That(rootFromDb.Composition!.Text, Is.EqualTo(entity.Composition.Text));
        }

        const string updatedText = "Composition Update";
        var entityUpdate = new RootNode
        {
            Id = entity.Id,
            Composition = new Entity
            {
                Id = entity.Composition.Id,
                Text = updatedText
            }
        };

        const string updatedAfterLoad = "Updated after load";
        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entityUpdate);

            var compositionEntityFromDb = await dbContext.Set<Entity>()
                .SingleAsync(e => e.Id == entityUpdate.Composition.Id);

            Assert.That(ReferenceEquals(entityUpdate.Composition, compositionEntityFromDb));

            compositionEntityFromDb.Text = updatedAfterLoad;
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(n => n.Composition)
                .SingleAsync();

            Assert.That(rootFromDb, Is.Not.Null);
            Assert.That(rootFromDb.Composition!.Text, Is.EqualTo(updatedAfterLoad));
        }
    }

    [Test]
    public async Task _02_ShowcaseTrackingBehaviorAfterBusinessLogic_ForCompositions()
    {
        var entity = new RootNode
        {
            Composition = new Entity
            {
                Text = "Composition Init"
            }
        };

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(n => n.Composition)
                .SingleAsync();

            Assert.That(rootFromDb, Is.Not.Null);
            Assert.That(rootFromDb.Composition!.Text, Is.EqualTo(entity.Composition.Text));
        }

        const string updatedText = "Composition Update";
        var entityUpdate = new RootNode
        {
            Id = entity.Id,
            Composition = new Entity
            {
                Id = entity.Composition.Id,
                Text = updatedText
            },
            Aggregations =
            {
                new Entity
                {
                    Id = entity.Composition.Id,
                    Text = "Should not be updated"
                }
            }
        };

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var compositionFromDb = await dbContext.Set<Entity>().SingleAsync(e => e.Id == entityUpdate.Composition.Id);
            compositionFromDb.Text = "This should throw";
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.ThrowsAsync<EntityAlreadyTrackedException>(async () =>
                 await graphTracker.TrackGraphAsync(entityUpdate));
        }
    }

    [Test]
    public async Task _03_ShowcaseTrackedRootNode_BeforeTrackGraphCall_Throws()
    {
        var rootNode = new RootNode
        {
            Text = "Root 1"
        };

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = new RootNode
        {
            Id = rootNode.Id,
            Text = "Root 1 Update"
        };

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            await dbContext.RootNodes.SingleAsync(r => r.Id == rootUpdate.Id);
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.ThrowsAsync<EntityAlreadyTrackedException>(async () =>
                 await graphTracker.TrackGraphAsync(rootUpdate));
        }
    }
}