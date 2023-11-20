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
    public async Task _03_SetNull_OnReferenceNavigation_ForRequiredOneToManyAggregationRelationship_DoesNothing()
    {
        var aggregationItem = new OneItem();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(aggregationItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItem = new DependentManyItemWithRequiredAggregation
        {
            RequiredAggregation = new OneItem
            {
                Id = aggregationItem.Id
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithRequiredAggregation
        {
            Id = manyItem.Id,
            RequiredAggregation = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<DependentManyItemWithRequiredAggregation>()
                .Include(i => i.RequiredAggregation)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemFromDb, Is.Not.Null);
            Assert.That(manyItemFromDb!.RequiredAggregation, Is.Not.Null);
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
    public async Task _05_SetNull_OnReferenceNavigation_ForOptionalOneToManyAggregationRelationship_SeversRelationship()
    {
        var aggregationItem = new OneItem();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(aggregationItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItem = new DependentManyItemWithOptionalAggregation
        {
            OptionalAggregation = new OneItem
            {
                Id = aggregationItem.Id
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithOptionalAggregation
        {
            Id = manyItem.Id,
            OptionalAggregation = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<DependentManyItemWithOptionalAggregation>()
                .Include(i => i.OptionalAggregation)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemFromDb, Is.Not.Null);
            Assert.That(manyItemFromDb!.OptionalAggregation, Is.Null);
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var oneItemFromDb = await dbContext.Set<OneItem>()
                .SingleOrDefaultAsync(i => i.Id == manyItem.OptionalAggregation.Id);

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
        _06_SetNull_WithNotNullValueInFk_OnReferenceNavigation_ForOptionalOneToManyAggregationRelationship_UsesFkValueOnly()
    {
        var aggregationItem = new OneItem();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(aggregationItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItem = new DependentManyItemWithDefinedFkOptionalAggregation
        {
            OptionalAggregationId = aggregationItem.Id,
            OptionalAggregation = new OneItem
            {
                Id = aggregationItem.Id
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItem);
            await dbContext.SaveChangesAsync();
        }

        var manyItemUpdate = new DependentManyItemWithDefinedFkOptionalAggregation
        {
            Id = manyItem.Id,
            OptionalAggregationId = aggregationItem.Id,
            OptionalAggregation = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(manyItemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<DependentManyItemWithDefinedFkOptionalAggregation>()
                .Include(i => i.OptionalAggregation)
                .SingleOrDefaultAsync(i => i.Id == manyItem.Id);

            Assert.That(manyItemFromDb, Is.Not.Null);
            Assert.That(manyItemFromDb!.OptionalAggregation, Is.Not.Null);
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var oneItemFromDb = await dbContext.Set<OneItem>()
                .SingleOrDefaultAsync(i => i.Id == manyItem.OptionalAggregation.Id);

            Assert.That(oneItemFromDb, Is.Not.Null);
        }
    }
}