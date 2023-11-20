using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToOne;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships;

public class OneToOneTests : TestBase<RelationshipTestsDbContext>
{
    [Test]
    public async Task
        _01_AddingNewEntity_WithNullValueInReferenceNavigation_ForRequiredOneToOneCompositionRelationship_ThrowsException()
    {
        var item = new DependentItemWithRequiredComposition
        {
            RequiredPrincipal = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(item);
            // This is EF-Core default behavior.
            Assert.ThrowsAsync<InvalidOperationException>(async () => await dbContext.SaveChangesAsync());
        }
    }

    [Test]
    public async Task _02_SetNull_OnReferenceNavigation_ForRequiredOneToOneCompositionRelationship_DeletesDependent()
    {
        var item = new DependentItemWithRequiredComposition
        {
            RequiredPrincipal = new PrincipalItemForComposition()
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(item);
            await dbContext.SaveChangesAsync();
        }

        var itemUpdate = new DependentItemWithRequiredComposition
        {
            Id = item.Id,
            RequiredPrincipal = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(itemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var manyItemFromDb = await dbContext.Set<DependentItemWithRequiredComposition>()
                .Include(i => i.RequiredPrincipal)
                .SingleOrDefaultAsync(i => i.Id == item.Id);

            Assert.That(manyItemFromDb, Is.Not.Null);
            Assert.That(manyItemFromDb!.RequiredPrincipal, Is.Not.Null);
        }
    }

    [Test]
    public async Task _03_SetNull_OnReferenceNavigation_ForRequiredOneToOneAggregationRelationship_DeletesDependent()
    {
        var aggregationItem = new PrincipalItemForAggregation();

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            dbContext.Add(aggregationItem);
            await dbContext.SaveChangesAsync();
        }

        var item = new DependentItemWithRequiredAggregation
        {
            RequiredPrincipal = new PrincipalItemForAggregation
            {
                Id = aggregationItem.Id
            }
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(item);
            await dbContext.SaveChangesAsync();
        }

        var itemUpdate = new DependentItemWithRequiredAggregation
        {
            Id = item.Id,
            RequiredPrincipal = null
        };

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(itemUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new RelationshipTestsDbContext())
        {
            var itemFromDb = await dbContext.Set<DependentItemWithRequiredAggregation>()
                .Include(i => i.RequiredPrincipal)
                .SingleOrDefaultAsync(i => i.Id == item.Id);

            Assert.That(itemFromDb, Is.Not.Null);
            Assert.That(itemFromDb!.RequiredPrincipal, Is.Not.Null);
        }
    }
}