using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes;

public class KeyTypeTests : TestBase<KeyTypeTestsDbContext>
{
    private static dynamic[] _entitiesWithDifferentKeyTypes =
    {
        new EntityWithIntKey(),
        new EntityWithLongKey(),
        new EntityWithGuidKey(),
        new EntityWithStringKey(),
        new EntityWithByteArrayKey()
    };
    
    [Test]
    [TestCaseSource(nameof(_entitiesWithDifferentKeyTypes))]
    public async Task _01_Entities_WithDifferentKeyTypes_CanBeAdded(dynamic entity)
    {
        await TrackEntity_AssertNoError_AndExistsInDb(entity);
    }

    private async Task TrackEntity_AssertNoError_AndExistsInDb<T>(T entity) where T : class
    {
        await using (var dbContext = new KeyTypeTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(entity));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new KeyTypeTestsDbContext())
        {
            var entityFromDb = await dbContext.Set<T>()
                .SingleOrDefaultAsync();
            
            Assert.That(entityFromDb, Is.Not.Null);
        }
    }
}