using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToOneRelationship;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged;

public class OneToOneRelationships : TestBase<ForceUnchangedDbContext>
{
        [Test]
    public async Task
        _01_ManyToManyRelationships_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var itemOne = new OptionalOneItemOne();
        var itemTwo = new OptionalOneItemTwo();

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.AddRange(itemOne, itemTwo);
            await dbContext.SaveChangesAsync();
        }

        var itemOneUpdate = new OptionalOneItemOne()
        {
            Id = itemOne.Id,
            OptionalItem = new()
            {
                Id = itemTwo.Id
            }
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(itemOneUpdate));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var itemOneFromDb = await dbContext.Set<OptionalOneItemOne>()
                .Include(i => i.OptionalItem)
                .SingleAsync(i => i.Id == itemOne.Id);

            Assert.That(itemOneFromDb.OptionalItem, Is.Not.Null);
        }

        var itemTwoUpdate = new OptionalOneItemTwo()
        {
            Id = itemTwo.Id,
            OptionalItem = null
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(itemTwoUpdate));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var itemTwoFromDb = await dbContext.Set<OptionalOneItemTwo>()
                .Include(i => i.OptionalItem)
                .SingleAsync(i => i.Id == itemTwo.Id);

            Assert.That(itemTwoFromDb.OptionalItem, Is.Not.Null);
        }
        
        var itemTwoUpdate2 = new OptionalOneItemTwo()
        {
            Id = itemTwo.Id,
            OptionalItem = new()
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(itemTwoUpdate2));
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var itemTwoFromDb = await dbContext.Set<OptionalOneItemTwo>()
                .Include(i => i.OptionalItem)
                .SingleAsync(i => i.Id == itemTwo.Id);

            // New relationships can be created, only null values are ignored
            Assert.That(itemTwoFromDb.OptionalItem, Is.Not.Null);
        }
    }
}