using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps;

public class ConcurrencyStampTests : TestBase<ConcurrencyStampTestDbContext>
{
    private const string UPDATED_TEXT = "Text Update";

    [Test]
    public async Task _01_EntityWithValidConcurrencyStamp_IsUpdatedCorrectly()
    {
        var itemUpdate = await SaveItemAndCreateUpdateObjectAsync(true);

        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(itemUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var itemFromDb = await dbContext.Set<ItemWithConcurrencyStamp>().SingleAsync();
            Assert.That(itemFromDb.Text, Is.EqualTo(UPDATED_TEXT));
        }
    }
    
    [Test]
    public async Task _02_Update_OnEntityWithOutdatedConcurrencyStamp_ThrowsException()
    {
        var itemUpdate = await SaveItemAndCreateUpdateObjectAsync(false);
        
        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(itemUpdate);
            Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await dbContext.SaveChangesAsync());
        }
    }

    private async Task<ItemWithConcurrencyStamp> SaveItemAndCreateUpdateObjectAsync(
        bool shouldHaveValidConcurrencyToken)
    {
        var item = new ItemWithConcurrencyStamp()
        {
            Text = "Init"
        };
        
        await using (var dbContext = new ConcurrencyStampTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(item);
            await dbContext.SaveChangesAsync();
            Assert.That(item.ConcurrencyToken, Is.GreaterThan(0));
        }

        return new ItemWithConcurrencyStamp()
        {
            Id = item.Id,
            Text = UPDATED_TEXT,
            ConcurrencyToken = shouldHaveValidConcurrencyToken ? item.ConcurrencyToken : item.ConcurrencyToken - 1
        };
    }
}