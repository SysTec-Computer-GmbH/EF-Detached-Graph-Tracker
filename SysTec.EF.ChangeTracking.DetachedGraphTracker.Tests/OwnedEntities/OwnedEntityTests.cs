using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities;

public class OwnedEntityTests : TestBase<OwnedEntityTestsDbContext>
{
    [Test]
    public async Task _01_InsertAndUpdateEntityWithOwnedEntityReference_ShouldStoreOwnedValues()
    {
        var entityA = new SubType_A
        {
            Text = "Post SubType_A",
            OwnedType = new OwnedType
            {
                Text = "Post Owned in SubType_A"
            }
        };

        var entityB = new SubType_B
        {
            Text = "Post SubType_B",
            OwnedType = new OwnedType
            {
                Text = "Post Owned in SubType_B"
            }
        };

        await using (var dbContext = new OwnedEntityTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entityA);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new OwnedEntityTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entityB);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new OwnedEntityTestsDbContext())
        {
            var entityAFromDb = await dbContext.SubTypeAs.SingleAsync();
            Assert.That(entityAFromDb.OwnedType, Is.Not.Null);
            Assert.That(entityAFromDb.OwnedType.Text, Is.EqualTo(entityA.OwnedType.Text));

            var entityBFromDb = await dbContext.SubTypeBs.SingleAsync();
            Assert.That(entityBFromDb.OwnedType, Is.Not.Null);
            Assert.That(entityBFromDb.OwnedType.Text, Is.EqualTo(entityB.OwnedType.Text));
        }

        var updatedEntityA = new SubType_A
        {
            Id = entityA.Id,
            Text = "Updated SubType_A",
            OwnedType = new OwnedType
            {
                Text = "Updated Owned in SubType_A"
            }
        };

        var updatedEntityB = new SubType_B
        {
            Id = entityB.Id,
            Text = "Updated SubType_B",
            OwnedType = new OwnedType
            {
                Text = "Updated Owned in SubType_B"
            }
        };

        await using (var dbContext = new OwnedEntityTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(updatedEntityA);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new OwnedEntityTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(updatedEntityB);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new OwnedEntityTestsDbContext())
        {
            var entityAFromDb = await dbContext.SubTypeAs.SingleAsync();
            Assert.That(entityAFromDb.OwnedType, Is.Not.Null);
            Assert.That(entityAFromDb.OwnedType.Text, Is.EqualTo(updatedEntityA.OwnedType.Text));

            var entityBFromDb = await dbContext.SubTypeBs.SingleAsync();
            Assert.That(entityBFromDb.OwnedType, Is.Not.Null);
            Assert.That(entityBFromDb.OwnedType.Text, Is.EqualTo(updatedEntityB.OwnedType.Text));
        }
    }

    [Test]
    public async Task _02_InsertAndUpdateEntityWithOwnedEntityCollection_ShouldThrow()
    {
        var entity = new TypeWithOwnedCollection
        {
            Text = "Entity",
            OwnedTypes = new List<OwnedType>
            {
                new()
                {
                    Text = "Owned 1"
                },
                new()
                {
                    Text = "Owned 2"
                }
            }
        };

        await using (var dbContext = new OwnedEntityTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.ThrowsAsync<OwnedCollectionNotSupportedException>(async () =>
                 await graphTracker.TrackGraphAsync(entity));
            await dbContext.SaveChangesAsync();
        }

        // Support for owned collections is not yet implemented and has no priority.
        // await using (var dbContext = new OwnedEntityTestsDbContext())
        // {
        //     var entityFromDb = await dbContext.TypeWithOwnedCollections.SingleAsync();
        //     Assert.Multiple(() =>
        //     {
        //         Assert.That(entityFromDb.OwnedTypes, Is.Not.Null);
        //         Assert.That(entityFromDb.OwnedTypes, Has.Count.EqualTo(2));
        //         Assert.That(entityFromDb.OwnedTypes[0].Text, Is.EqualTo(entity.OwnedTypes[0].Text));
        //         Assert.That(entityFromDb.OwnedTypes[1].Text, Is.EqualTo(entity.OwnedTypes[1].Text));
        //     });
        // }
        //
        // var entityUpdate = (TypeWithOwnedCollection) entity.Clone();
        // entityUpdate.OwnedTypes[0].Text = "Updated Owned 1";
        // entityUpdate.OwnedTypes[1].Text = "Updated Owned 2";
        //
        // await using (var dbContext = new OwnedEntityTestsDbContext())
        // {
        //     var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraph(entityUpdate);
        //     await dbContext.SaveChangesAsync();
        // }
        //
        // await using (var dbContext = new OwnedEntityTestsDbContext())
        // {
        //     var entityFromDb = await dbContext.TypeWithOwnedCollections.SingleAsync();
        //     Assert.Multiple(() =>
        //     {
        //         Assert.That(entityFromDb.OwnedTypes, Is.Not.Null);
        //         Assert.That(entityFromDb.OwnedTypes, Has.Count.EqualTo(2));
        //         Assert.That(entityFromDb.OwnedTypes[0].Text, Is.EqualTo(entityUpdate.OwnedTypes[0].Text));
        //         Assert.That(entityFromDb.OwnedTypes[1].Text, Is.EqualTo(entityUpdate.OwnedTypes[1].Text));
        //     });
        // }
    }
}