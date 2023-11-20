using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.Association.Collection;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.Association.Reference;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes;

public class UpdateAssociationOnlyTests : TestBase<AttributeTestsDbContext>
{
    [Test]
    public async Task _01_AssociationAttribute_IsRecognized_OnCollection_WithBackReference_ToAbstractBaseType()
    {
        var concreteType = new ConcreteTypeWithConcreteCollection()
        {
            Items = new()
            {
                new()
            }
        };

        using (var dbContext = new AttributeTestsDbContext())
        {
            var entry = dbContext.Entry(concreteType);
            var collectionEntry = entry.Collection(o => o.Items);
            // Assert that relationship for test case is defined correctly
            Assert.That(collectionEntry.Metadata.PropertyInfo!.DeclaringType, Is.EqualTo(typeof(AbstractTypeWithAbstractCollection)));
            
            dbContext.ChangeTracker.TrackGraph(concreteType, Callback);   
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            dbContext.Add(concreteType);
            await dbContext.SaveChangesAsync();
        }

        var concreteTypeUpdate = new ConcreteTypeWithConcreteCollection()
        {
            Id = concreteType.Id,
            Items = new()
            {
                new()
                {
                    Id = concreteType.Items.First().Id,
                    Text = "Updated"
                }
            }
        };
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var graphHandler = new DetachedGraphTracker(dbContext);
            await graphHandler.TrackGraphAsync(concreteTypeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var updatedConcreteType = await dbContext.AssociationConcreteCollectionTypes
                .Include(o => o.Items)
                .SingleAsync(o => o.Id == concreteType.Id);
            
            Assert.That(updatedConcreteType.Items.Count, Is.EqualTo(1));
            Assert.That(updatedConcreteType.Items.First().Text, Is.Null);
        }
    }
    
    [Test]
    public async Task _02_AssociationAttribute_IsRecognized_OnReference_WithBackReference_ToAbstractBaseType()
    {
        var concreteType = new ConcreteTypeWithConcreteReference()
        {
            Item = new()
        };

        using (var dbContext = new AttributeTestsDbContext())
        {
            var entry = dbContext.Entry(concreteType);
            var collectionEntry = entry.Reference(o => o.Item);
            // Assert that relationship for test case is defined correctly
            Assert.That(collectionEntry.Metadata.PropertyInfo!.DeclaringType, Is.EqualTo(typeof(AbstractTypeWithAbstractReference)));
            
            dbContext.ChangeTracker.TrackGraph(concreteType, Callback);   
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            dbContext.Add(concreteType);
            await dbContext.SaveChangesAsync();
        }
        
        var concreteTypeUpdate = new ConcreteTypeWithConcreteReference()
        {
            Id = concreteType.Id,
            Item = new()
            {
                Id = concreteType.Id,
                Text = "Update"
            }
        };
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var graphHandler = new DetachedGraphTracker(dbContext);
            await graphHandler.TrackGraphAsync(concreteTypeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new AttributeTestsDbContext())
        {
            var updatedConcreteType = await dbContext.AssociationConcreteReferenceTypes
                .Include(o => o.Item)
                .SingleAsync(o => o.Id == concreteType.Id);
            
            Assert.That(updatedConcreteType.Item, Is.Not.Null);
            Assert.That(updatedConcreteType.Item.Text, Is.Null);
        }
    }
    

    private void Callback(EntityEntryGraphNode node)
    {
        node.Entry.State = node.Entry.IsKeySet ? EntityState.Modified : EntityState.Added;

        if (node.Entry.Entity.GetType() == typeof(ReferenceItemWithBackreferenceToAbstractType))
        {
            Assert.That(node.InboundNavigationHasUpdateAssociationOnlyAttribute(), Is.True);
        }
    }
}