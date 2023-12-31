using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase;

public class PropertyInitializerTests : TestBase<ImplementationShowcaseTestsDbContext>
{
    [Test]
    public async Task _01_EmptyCompositionNavigation_WithNullAsForeignKey_CreatesEntityInDb()
    {
        var entity = new EntityWithPropertiesNewInitializer
        {
            Text = "Text",
            AssociationEntity = null
        };

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var compositionEntityFromDb = await dbContext.Set<Entity>()
                .SingleAsync();

            Assert.That(compositionEntityFromDb, Is.Not.Null);
            Assert.That(compositionEntityFromDb.Id, Is.GreaterThan(0));
        }
    }

    [Test]
    public async Task _02_EmptyAssociationNavigation_WithNullAsForeignKey_DoesPersistEntityInDb()
    {
        // In this case the entity is not created in the database because the key of the association entity is 0.
        // Associations with a key of 0 throw an exception when no further parameter is passed to the Association Attribute.

        var entity = new EntityWithPropertiesNewInitializer
        {
            Text = "Text",
            CompositionEntity = null
        };

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); 
            Assert.ThrowsAsync<AddedAssociationEntryException>(async () => await graphTracker.TrackGraphAsync(entity));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ImplementationShowcaseTestsDbContext())
        {
            var associationEntityFromDb = await dbContext.Set<Entity>()
                .SingleOrDefaultAsync();

            Assert.That(associationEntityFromDb, Is.Null);
        }
    }
}