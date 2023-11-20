using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Collection;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Reference;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes;

public class ForceRelationshipUnchangedAttributeTests : TestBase<AttributeTestsDbContext>
{
    [Test]
    public async Task _01_ForceRelationshipUnchangedAttribute_IsRecognized_OnCollection_WithBackReference_ToAbstractBaseType()
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
            Assert.That(collectionEntry.HasForceUnchangedAttribute(), Is.True);
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            dbContext.Add(concreteType);
            await dbContext.SaveChangesAsync();
        }

        var concreteType2 = new ConcreteTypeWithConcreteCollection()
        {
            Id = concreteType.Id,
            Items = new()
            {
                new()
            }
        };
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            dbContext.Update(concreteType2);
            await dbContext.SaveChangesAsync();
        }

        var concreteTypeUpdate = new ConcreteTypeWithConcreteCollection()
        {
            Id = concreteType.Id
        };

        await using (var dbContext = new AttributeTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(concreteTypeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var itemOne = await dbContext.Set<CollectionItemWithBackreferenceToAbstractType>()
                .Include(i => i.AbstractType)
                .SingleAsync(o => o.Id == concreteType.Items.First().Id);
            
            Assert.That(itemOne.AbstractType, Is.Not.Null);
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var itemTwo = await dbContext.Set<CollectionItemWithBackreferenceToAbstractType>()
                .Include(i => i.AbstractType)
                .SingleAsync(o => o.Id == concreteType2.Items.First().Id);
            
            Assert.That(itemTwo.AbstractType, Is.Not.Null);
        }
    }
    
    [Test]
    public async Task _02_ForceRelationshipUnchangedAttribute_IsRecognized_OnReference_WithBackReference_ToAbstractBaseType()
    {
        var concreteType = new ConcreteTypeWithConcreteReference()
        {
            Item = new()
        };

        await using (var dbContext = new AttributeTestsDbContext())
        {
            var entry = dbContext.Entry(concreteType);
            var referenceEntry = entry.Reference(o => o.Item);
            // Assert that relationship for test case is defined correctly
            Assert.That(referenceEntry.Metadata.PropertyInfo!.DeclaringType, Is.EqualTo(typeof(AbstractTypeWithAbstractReference)));
            Assert.That(referenceEntry.HasForceUnchangedAttribute(), Is.True);
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            dbContext.Add(concreteType);
            await dbContext.SaveChangesAsync();
        }

        var concreteTypeUpdate = new ConcreteTypeWithConcreteReference()
        {
            Id = concreteType.Id
        };
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(concreteTypeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var concreteTypeFromDb = await dbContext.Set<ConcreteTypeWithConcreteReference>()
                .Include(ct => ct.Item)
                .SingleAsync(o => o.Id == concreteType.Id);
            
            Assert.That(concreteTypeFromDb.Item, Is.Not.Null);
        }
    }
}