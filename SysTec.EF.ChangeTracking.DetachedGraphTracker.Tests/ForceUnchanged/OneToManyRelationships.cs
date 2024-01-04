using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged;

public class OneToManyRelationships : TestBase<ForceUnchangedDbContext>
{
    
    [Test]
    public async Task _01_RequiredReferenceNavigationValues_InOneToManyRelationship_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var root = new RootWithRequiredReference()
        {
            RequiredItem = new()
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = new RootWithRequiredReference()
        {
            Id = root.Id
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var rootFromDb = await dbContext.Set<RootWithRequiredReference>()
                .Include(x => x.RequiredItem)
                .SingleAsync(x => x.Id == root.Id);

            Assert.That(rootFromDb, Is.Not.Null);
            Assert.That(rootFromDb.RequiredItem, Is.Not.Null);
        }
    }
    
    [Test]
    public async Task _02_OptionalReferenceNavigationValues_InOneToManyRelationship_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var root = new RootWithOptionalReference()
        {
            OptionalItem = new()
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = new RootWithOptionalReference()
        {
            Id = root.Id
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var rootFromDb = await dbContext.Set<RootWithOptionalReference>()
                .Include(x => x.OptionalItem)
                .SingleAsync(x => x.Id == root.Id);

            Assert.That(rootFromDb, Is.Not.Null);
            Assert.That(rootFromDb.OptionalItem, Is.Not.Null);
        }
    }
    
    [Test]
    public async Task _03_RequiredCollectionNavigationValues_InOneToManyRelationship_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var root = new RootWithRequiredCollection()
        {
            RequiredItems = new()
            {
                new()
            }
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = new RootWithRequiredCollection()
        {
            Id = root.Id,
            RequiredItems = new()
            {
                new()
            }
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var rootFromDb = await dbContext.Set<RootWithRequiredCollection>()
                .Include(x => x.RequiredItems)
                .SingleAsync(x => x.Id == root.Id);

            Assert.That(rootFromDb, Is.Not.Null);
            // New relationships can be added anyway
            Assert.That(rootFromDb.RequiredItems, Has.Count.EqualTo(2));
        }
    }
    
    [Test]
    public async Task _04_OptionalCollectionNavigationValues_InOneToManyRelationship_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var root = new RootWithOptionalCollection()
        {
            OptionalItems = new()
            {
                new()
            }
        };

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }
        
        var rootUpdate = new RootWithOptionalCollection()
        {
            Id = root.Id,
            OptionalItems = new()
            {
                new()
            }
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var rootFromDb = await dbContext.Set<RootWithOptionalCollection>()
                .Include(x => x.OptionalItems)
                .SingleAsync(x => x.Id == root.Id);

            Assert.That(rootFromDb, Is.Not.Null);
            // New relationships can be added anyway
            Assert.That(rootFromDb.OptionalItems, Has.Count.EqualTo(2));
        }
    }

    [Test]
    public async Task _05_OptionalReferenceNavigationValues_WithDefinedFk_AndPreviouslyTrackedBackreference_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var root = new RootWithOptionalReferenceWithFkWithBackreference();
        var root2 = new RootWithOptionalReferenceWithFkWithBackreference();
        var optionalItem = new OptionalReferenceItemWithBackreference();

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.AddRange(root, root2, optionalItem);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = new RootWithOptionalReferenceWithFkWithBackreference()
        {
            Id = root.Id,
            OptionalItem = new()
            {
                Id = optionalItem.Id
            }
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async() => await graphTracker.TrackGraphAsync(rootUpdate));
            await dbContext.SaveChangesAsync();
        }

        var root2Update = new RootWithOptionalReferenceWithFkWithBackreference()
        {
            Id = root2.Id,
            OptionalItem = new()
            {
                Id = optionalItem.Id
            }
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async() => await graphTracker.TrackGraphAsync(root2Update));
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var optionalItemFromDb = await dbContext.Set<OptionalReferenceItemWithBackreference>()
                .Include(i => i.Roots)
                .SingleAsync(i => i.Id == optionalItem.Id);
            
            Assert.That(optionalItemFromDb.Roots, Has.Count.EqualTo(2));
        }
    }
    
    [Test]
    public async Task _06_OptionalCollectionNavigationValues_WithDefinedFk_AndPreviouslyTrackedBackreference_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var root = new RootWithOptionalCollectionWithFkWithBackreference();

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = new RootWithOptionalCollectionWithFkWithBackreference()
        {
            Id = root.Id,
            OptionalItems = new()
            {
                new()
            }
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async() => await graphTracker.TrackGraphAsync(rootUpdate));
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var rootFromDb = await dbContext.Set<RootWithOptionalCollectionWithFkWithBackreference>()
                .Include(x => x.OptionalItems)
                .SingleAsync(x => x.Id == root.Id);
            
            Assert.That(rootFromDb.OptionalItems, Has.Count.EqualTo(1));
        }

        var optionalItemUpdate = new OptionalCollectionItemWithBackreference()
        {
            Id = rootUpdate.OptionalItems.First().Id
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async() => await graphTracker.TrackGraphAsync(optionalItemUpdate));
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var optionalItemFromDb = await dbContext.Set<OptionalCollectionItemWithBackreference>()
                .Include(i => i.Root)
                .SingleAsync(i => i.Id == optionalItemUpdate.Id);
            
            Assert.That(optionalItemFromDb.Root, Is.Not.Null);
        }
    }
    
    [Test]
    public async Task _07_OptionalReferenceNavigationValues_InOneToManyRelationship_InsideOwnedType_AreNotAffectedByChanges_WhenAttributeIsUsed()
    {
        var rootInit = new RootWithOwnedType()
        {
            Id = 1,
            Owned = new()
            {
                OptionalItem = new()
            }
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            dbContext.Add(rootInit);
            await dbContext.SaveChangesAsync();
        }
        
        Assert.That(rootInit.Id, Is.GreaterThan(0));
        Assert.That(rootInit.Owned.OptionalItem.Id, Is.GreaterThan(0));

        var rootUpdate = new RootWithOwnedType()
        {
            Id = rootInit.Id,
            Owned = new()
            {
                OptionalItem = null
            }
        };
        
        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ForceUnchangedDbContext())
        {
            var updateFromDb = await dbContext.Set<RootWithOwnedType>()
                .Include(x => x.Owned.OptionalItem)
                .SingleAsync(x => x.Id == rootInit.Id);
            
            Assert.That(updateFromDb.Owned.OptionalItem, Is.Not.Null);
        }
    }

}