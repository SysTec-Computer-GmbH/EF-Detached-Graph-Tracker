using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships;

public class OneToManyTests : TestBase<RelationshipTestsDbContext>
{
    [Test]
    public async Task
        _01_AddingNewEntity_WithNullValueInReferenceNavigation_ForRequiredOneToManyCompositionRelationship_ThrowsException()
    {
        var manyItem = new DependentManyItemWithRequiredComposition
        {
            RequiredComposition = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItem);
            // This is EF-Core default behavior.
            Assert.ThrowsAsync<DbUpdateException>(async () => await dbContext.SaveChangesAsync());
        }
    }

    [Test]
    public async Task _02_SetNull_OnReferenceNavigation_ForRequiredOneToManyCompositionRelationship_DoesNothing()
    {
        var manyItem = new DependentManyItemWithRequiredComposition
        {
            RequiredComposition = new OneItem()
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithRequiredComposition
        {
            Id = manyItem.Id,
            RequiredComposition = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemsFromDb = await dbContext.Set<DependentManyItemWithRequiredComposition>()
                .Include(i => i.RequiredComposition)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemsFromDb, Is.Not.Null);
            Assert.That(manyItemsFromDb!.RequiredComposition, Is.Not.Null);
        }
    }

    [Test]
    public async Task _03_SetNull_OnReferenceNavigation_ForRequiredOneToManyAssociationRelationship_DoesNothing()
    {
        var associationItem = new OneItem();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(associationItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItem = new DependentManyItemWithRequiredAssociation
        {
            RequiredAssociation = new OneItem
            {
                Id = associationItem.Id
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithRequiredAssociation
        {
            Id = manyItem.Id,
            RequiredAssociation = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<DependentManyItemWithRequiredAssociation>()
                .Include(i => i.RequiredAssociation)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemFromDb, Is.Not.Null);
            Assert.That(manyItemFromDb!.RequiredAssociation, Is.Not.Null);
        }
    }

    [Test]
    public async Task _04_SetNull_OnReferenceNavigation_ForOptionalOneToManyCompositionRelationship_SeversRelationship()
    {
        var manyItem = new DependentManyItemWithOptionalComposition
        {
            OptionalComposition = new OneItem()
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithOptionalComposition
        {
            Id = manyItem.Id,
            OptionalComposition = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemsFromDb = await dbContext.Set<DependentManyItemWithOptionalComposition>()
                .Include(oc => oc.OptionalComposition)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemsFromDb, Is.Not.Null);
            Assert.That(manyItemsFromDb!.OptionalComposition, Is.Null);
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var oneItemFromDb = await dbContext.Set<OneItem>()
                .SingleOrDefaultAsync(i => i.Id == manyItem.OptionalComposition.Id);

            Assert.That(oneItemFromDb, Is.Not.Null);
        }
    }

    [Test]
    public async Task _05_SetNull_OnReferenceNavigation_ForOptionalOneToManyAssociationRelationship_SeversRelationship()
    {
        var associationItem = new OneItem();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(associationItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItem = new DependentManyItemWithOptionalAssociation
        {
            OptionalAssociation = new OneItem
            {
                Id = associationItem.Id
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithOptionalAssociation
        {
            Id = manyItem.Id,
            OptionalAssociation = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<DependentManyItemWithOptionalAssociation>()
                .Include(i => i.OptionalAssociation)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemFromDb, Is.Not.Null);
            Assert.That(manyItemFromDb!.OptionalAssociation, Is.Null);
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var oneItemFromDb = await dbContext.Set<OneItem>()
                .SingleOrDefaultAsync(i => i.Id == manyItem.OptionalAssociation.Id);

            Assert.That(oneItemFromDb, Is.Not.Null);
        }
    }

    [Test]
    public async Task
        _06_SetNull_WithNotNullValueInFk_OnReferenceNavigation_ForOptionalOneToManyCompositionRelationship_UsesFkValueOnly()
    {
        var manyItem = new DependentManyItemWithDefinedFkOptionalComposition
        {
            OptionalComposition = new OneItem()
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithDefinedFkOptionalComposition
        {
            Id = manyItem.Id,
            OptionalCompositionId = manyItem.OptionalComposition.Id,
            OptionalComposition = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemsFromDb = await dbContext.Set<DependentManyItemWithDefinedFkOptionalComposition>()
                .Include(oc => oc.OptionalComposition)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemsFromDb, Is.Not.Null);
            Assert.That(manyItemsFromDb!.OptionalComposition, Is.Not.Null);
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var oneItemFromDb = await dbContext.Set<OneItem>()
                .SingleOrDefaultAsync(i => i.Id == manyItem.OptionalComposition.Id);

            Assert.That(oneItemFromDb, Is.Not.Null);
        }
    }

    [Test]
    public async Task
        _06_SetNull_WithNotNullValueInFk_OnReferenceNavigation_ForOptionalOneToManyAssociationRelationship_UsesFkValueOnly()
    {
        var associationItem = new OneItem();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(associationItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItem = new DependentManyItemWithDefinedFkOptionalAssociation
        {
            OptionalAssociationId = associationItem.Id,
            OptionalAssociation = new OneItem
            {
                Id = associationItem.Id
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithDefinedFkOptionalAssociation
        {
            Id = manyItem.Id,
            OptionalAssociationId = associationItem.Id,
            OptionalAssociation = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<DependentManyItemWithDefinedFkOptionalAssociation>()
                .Include(i => i.OptionalAssociation)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemFromDb, Is.Not.Null);
            Assert.That(manyItemFromDb!.OptionalAssociation, Is.Not.Null);
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var oneItemFromDb = await dbContext.Set<OneItem>()
                .SingleOrDefaultAsync(i => i.Id == manyItem.OptionalAssociation.Id);

            Assert.That(oneItemFromDb, Is.Not.Null);
        }
    }
}