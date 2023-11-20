using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps;

public class ConcurrencyStampForceAggregationTests : TestBase<ConcurrencyStampTestDbContext>
{
    private const string INIT_TEXT = "Text Update";

    [Test]
    public async Task _01_Entity_InForceAggregationReferenceNavigation_WithValidConcurrencyStamp_IsUpdatedCorrectly()
    {
        var root = await SaveItemAndCreateRootObjectAsync(true);

        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var itemFromDb = await dbContext.Set<ItemWithConcurrencyStamp>().SingleAsync();
            Assert.That(itemFromDb.Text, Is.EqualTo(INIT_TEXT));
        }
    }

    [Test]
    public async Task
        _02_Entity_InForceAggregationReferenceNavigation_WithOutdatedConcurrencyStamp_DoesNotThrowException()
    {
        var root = await SaveItemAndCreateRootObjectAsync(false);

        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(root);
            Assert.DoesNotThrowAsync(async () => await dbContext.SaveChangesAsync());
        }

        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var itemFromDb = await dbContext.Set<ItemWithConcurrencyStamp>().SingleAsync();
            Assert.That(itemFromDb.Text, Is.EqualTo(INIT_TEXT));
        }
    }

    private async Task<RootWithConcurrencyItemInForceAggregationReference> SaveItemAndCreateRootObjectAsync(bool shouldHaveValidConcurrencyToken)
    {
        var item = new ItemWithConcurrencyStamp()
        {
            Text = INIT_TEXT
        };

        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(item);
            await dbContext.SaveChangesAsync();
            Assert.That(item.ConcurrencyToken, Is.GreaterThan(0));
        }

        return new RootWithConcurrencyItemInForceAggregationReference()
        {
            ItemWithConcurrencyStamp = new ItemWithConcurrencyStamp()
            {
                Id = item.Id,
                Text = "Updated",
                ConcurrencyToken = shouldHaveValidConcurrencyToken ? item.ConcurrencyToken : item.ConcurrencyToken - 1
            }
        };
    }
}