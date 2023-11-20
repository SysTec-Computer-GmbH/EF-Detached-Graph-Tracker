using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.ManyToManyRelationships;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged;

public class ManyToManyRelationships : TestBase<ForceUnchangedDbContext>
{
    [Test]
    public async Task
        _01_ManyToManyRelationships_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var itemOne1 = new ManyItemOne();
        var itemOne2 = new ManyItemOne();
        var itemTwo = new ManyItemTwo();

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.AddRange(itemOne1, itemOne2, itemTwo);
            await dbContext.SaveChangesAsync();
        }

        var itemOne1Update = new ManyItemOne()
        {
            Id = itemOne1.Id,
            OptionalItems = new()
            {
                new()
                {
                    Id = itemTwo.Id
                }
            }
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(itemOne1Update));
            await dbContext.SaveChangesAsync();
        }

        var itemOne2Update = new ManyItemOne()
        {
            Id = itemOne2.Id,
            OptionalItems = new()
            {
                new()
                {
                    Id = itemTwo.Id
                }
            }
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(itemOne2Update));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var itemTwoFromDb = await dbContext.Set<ManyItemTwo>()
                .Include(i => i.OptionalItems)
                .SingleAsync(i => i.Id == itemTwo.Id);

            Assert.That(itemTwoFromDb.OptionalItems, Has.Count.EqualTo(2));
        }

        var itemTwoUpdate = new ManyItemTwo()
        {
            Id = itemTwo.Id,
            OptionalItems = new()
            {
                new()
            }
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(itemTwoUpdate));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var itemTwoFromDb = await dbContext.Set<ManyItemTwo>()
                .Include(i => i.OptionalItems)
                .SingleAsync(i => i.Id == itemTwo.Id);

            // The old relationships are preserved although the list only contains a new relationship.
            Assert.That(itemTwoFromDb.OptionalItems, Has.Count.EqualTo(3));
        }
    }
}