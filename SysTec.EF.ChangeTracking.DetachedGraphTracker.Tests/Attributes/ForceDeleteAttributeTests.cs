using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceDelete.Collection;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes;

public class ForceDeleteAttributeTests : TestBase<AttributeTestsDbContext>
{
    [Test]
    public async Task _01_ForceDeleteAttribute_IsRecognized_OnCollection_WithBackReference_ToAbstractBaseType()
    {
        var concreteType = new ConcreteTypeWithConcreteCollection()
        {
            Items = new()
            {
                new()
            }
        };

        await using (var dbContext = new AttributeTestsDbContext())
        {
            var entry = dbContext.Entry(concreteType);
            var collectionEntry = entry.Collection(o => o.Items);
            // Assert that relationship for test case is defined correctly
            Assert.That(collectionEntry.Metadata.PropertyInfo!.DeclaringType, Is.EqualTo(typeof(AbstractTypeWithAbstractCollection)));
            Assert.That(collectionEntry.HasForceDeleteAttribute(), Is.True);
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            dbContext.Add(concreteType);
            await dbContext.SaveChangesAsync();
        }

        var concreteTypeUpdate = new ConcreteTypeWithConcreteCollection()
        {
            Id = concreteType.Id,
        };
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var graphHandler = new DetachedGraphTracker(dbContext);
            await graphHandler.TrackGraphAsync(concreteTypeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var concreteTypeFromDb = await dbContext.ForceDeleteConcreteTypes
                .Include(o => o.Items)
                .SingleAsync(o => o.Id == concreteType.Id);
            
            Assert.That(concreteTypeFromDb.Items.Count, Is.EqualTo(0));
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var itemFromDb = await dbContext.Set<CollectionItemWithBackreferenceToAbstractType>().ToListAsync();
            Assert.That(itemFromDb.Count, Is.EqualTo(0));
        }
    }
}