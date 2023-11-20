using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.ManyToMany;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.Nested;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.OneToMany;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences;

public class BackreferenceTests : TestBase<BackreferenceTestDbContext>
{
    [Test]
    public async Task _01_OtherRelationships_InOneToMany_AreNotSevered_WhenCollectionBackreference_HasNoAttribute()
    {
        var manyItemOne = new ManyItem();
        var manyItemTwo = new ManyItem();
        var oneItem = new OneItem();

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            dbContext.AddRange(manyItemOne, manyItemTwo, oneItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemOneUpdate = new ManyItem()
        {
            Id = manyItemOne.Id,
            OneItem = new OneItem()
            {
                Id = oneItem.Id
            }
        };

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            dbContext.UpdateRange(manyItemOneUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        var manyItemTwoUpdate = new ManyItem()
        {
            Id = manyItemTwo.Id,
            OneItem = new OneItem()
            {
                Id = oneItem.Id
            }
        };
        
        await using (var dbContext = new BackreferenceTestDbContext())
        {
            dbContext.UpdateRange(manyItemTwoUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new BackreferenceTestDbContext())
        {
            var oneItemFromDb = await dbContext.Set<OneItem>().Include(x => x.ManyItems).SingleAsync(x => x.Id == oneItem.Id);
            Assert.That(oneItemFromDb.ManyItems, Has.Count.EqualTo(2));
        }

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(manyItemTwoUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new BackreferenceTestDbContext())
        {
            // Using the graph tracker the result should be the same as in the previous update,
            // although no ForceRelationshipUnchanged attribute is set
            var oneItemFromDb = await dbContext.Set<OneItem>().Include(x => x.ManyItems).SingleAsync(x => x.Id == oneItem.Id);
            Assert.That(oneItemFromDb.ManyItems, Has.Count.EqualTo(2));
        }
    }
    
    [Test]
    public async Task _02_OtherRelationships_InManyToMany_AreNotSevered_WhenCollectionBackreference_HasNoAttribute()
    {
        var manyItemOne1 = new ManyItemOne();
        var manyItemOne2 = new ManyItemOne();
        var manyItemTwo = new ManyItemTwo();

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            dbContext.AddRange(manyItemOne1, manyItemOne2, manyItemTwo);
            await dbContext.SaveChangesAsync();
        }

        var manyItemOne1Update = new ManyItemOne()
        {
            Id = manyItemOne1.Id,
            Items = new()
            {
                new()
                {
                    Id = manyItemTwo.Id
                }
            }
        };

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            dbContext.UpdateRange(manyItemOne1Update);
            await dbContext.SaveChangesAsync();
        }
        
        var manyItemOne2Update = new ManyItemOne()
        {
            Id = manyItemOne2.Id,
            Items = new()
            {
                new()
                {
                    Id = manyItemTwo.Id
                }
            }
        };
        
        await using (var dbContext = new BackreferenceTestDbContext())
        {
            dbContext.UpdateRange(manyItemOne2Update);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new BackreferenceTestDbContext())
        {
            var oneItemFromDb = await dbContext.Set<ManyItemTwo>().Include(x => x.Items).SingleAsync(x => x.Id == manyItemTwo.Id);
            Assert.That(oneItemFromDb.Items, Has.Count.EqualTo(2));
        }

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(manyItemOne2Update);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new BackreferenceTestDbContext())
        {
            // Using the graph tracker the result should be the same as in the previous update,
            // although no ForceRelationshipUnchanged attribute is set
            var oneItemFromDb = await dbContext.Set<ManyItemTwo>().Include(x => x.Items).SingleAsync(x => x.Id == manyItemTwo.Id);
            Assert.That(oneItemFromDb.Items, Has.Count.EqualTo(2));
        }
    }
    
    [Test]
    public async Task _03_Relationships_InNestedTree_WithBackreferenceOnAbstractBaseType_AreSeveredCorrectly()
    {
        var concreteType = new ConcreteTypeWithAbstractTypeCollection()
        {
            Items = new()
            {
                new ConcreteTypeWithAbstractTypeCollection()
                {
                    Items = new()
                    {
                        new ConcreteTypeWithAbstractTypeCollection()
                    }
                }
            }
        };

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            dbContext.Add(concreteType);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new BackreferenceTestDbContext())
        {
            var concreteTypeFromDb = await dbContext.Set<ConcreteTypeWithAbstractTypeCollection>()
                .Include(a => a.Items)
                .ThenInclude(x => ((ConcreteTypeWithAbstractTypeCollection)x).Items)
                .ThenInclude(x => ((ConcreteTypeWithAbstractTypeCollection)x).Items)
                .SingleAsync(p => p.Id == concreteType.Id);
            
            Assert.That(concreteTypeFromDb.Items, Has.Count.EqualTo(1));
            Assert.That(concreteTypeFromDb.Items[0], Is.TypeOf<ConcreteTypeWithAbstractTypeCollection>());
            Assert.That(((ConcreteTypeWithAbstractTypeCollection)concreteTypeFromDb.Items[0]).Items[0], Is.TypeOf<ConcreteTypeWithAbstractTypeCollection>());
        }

        var concreteTypeUpdate = (ConcreteTypeWithAbstractTypeCollection)concreteType.Clone();
        ((ConcreteTypeWithAbstractTypeCollection)concreteTypeUpdate.Items[0]).Items.Clear();
        
        await using(var dbContext = new BackreferenceTestDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(concreteTypeUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new BackreferenceTestDbContext())
        {
            var concreteTypeFromDb = await dbContext.Set<ConcreteTypeWithAbstractTypeCollection>()
                .Include(x => x.Items)
                .ThenInclude(x => ((ConcreteTypeWithAbstractTypeCollection)x).Items)
                .ThenInclude(x => ((ConcreteTypeWithAbstractTypeCollection)x).Items)
                .SingleAsync(p => p.Id == concreteType.Id);
            
            Assert.That(concreteTypeFromDb.Items, Has.Count.EqualTo(1));
            Assert.That(concreteTypeFromDb.Items[0], Is.TypeOf<ConcreteTypeWithAbstractTypeCollection>());
            Assert.That(((ConcreteTypeWithAbstractTypeCollection)concreteTypeFromDb.Items[0]).Items, Has.Count.EqualTo(0));
        }
    }

}