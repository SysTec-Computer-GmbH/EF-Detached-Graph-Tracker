using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.GeneratedValues.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.GeneratedValues.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.GeneratedValues;

public class GeneratedValueChangeTrackingTests : TestBase<GeneratedValueTestsDbContext>
{
    [Test]
    public async Task _01_AddingAnEntity_WithGeneratedValueAlways_ShouldNotThrow()
    {
        var entryWithGeneratedValue = new EntryWithGeneratedValue();

        await using (var dbContext = new GeneratedValueTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entryWithGeneratedValue);
            Assert.DoesNotThrowAsync(async () => await dbContext.SaveChangesAsync());
        }

        await using (var dbContext = new GeneratedValueTestsDbContext())
        {
            var entryFromDb = await dbContext.Set<EntryWithGeneratedValue>()
                .SingleAsync(x => x.Id == entryWithGeneratedValue.Id);

            Assert.That(entryFromDb.GeneratedValue, Is.EqualTo(10));
        }
    }

    [Test]
    public async Task _02_UpdatingAnEntity_WithGeneratedValueAlways_ShouldNotThrow()
    {
        var entryWithGeneratedValue = new EntryWithGeneratedValue();

        await using (var dbContext = new GeneratedValueTestsDbContext())
        {
            dbContext.Add(entryWithGeneratedValue);
            await dbContext.SaveChangesAsync();
        }

        var entryUpdate = new EntryWithGeneratedValue
        {
            Id = entryWithGeneratedValue.Id,
            GeneratedValue = entryWithGeneratedValue.GeneratedValue
        };

        await using (var dbContext = new GeneratedValueTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entryUpdate);
            Assert.DoesNotThrowAsync(async () => await dbContext.SaveChangesAsync());
        }
    }
}