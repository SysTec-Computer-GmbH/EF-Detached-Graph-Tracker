using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleSameTrackedEntriesTests;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class MultipleSameTrackedEntriesTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task
        _01_GraphWithEntry_AllInReferenceNavigations_FirstTrackedAsAggregation_ThenTrackedAsComposition_ThenTrackedAsAggregation_DoesNotThrow_AndUpdatesValuesCorrectly()
    {
        var entity = new Entity()
        {
            Text = "Init"
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        var root = new RootNodeWithReferenceNavigations()
        {
            A_Aggregation = (Entity)entity.Clone(),
            B_Composition = (Entity)entity.Clone(),
            C_Aggregation = (Entity)entity.Clone()
        };
        
        root.A_Aggregation.Text = nameof(root.A_Aggregation);
        root.B_Composition.Text = nameof(root.B_Composition);
        root.C_Aggregation.Text = nameof(root.C_Aggregation);
        
        Assert.That(ReferenceEquals(root.A_Aggregation, root.B_Composition), Is.False);
        
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(root));
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var entityFromDb = await dbContext.Set<Entity>().SingleAsync(e => e.Id == entity.Id);
            
            Assert.That(entityFromDb.Text, Is.EqualTo(nameof(root.B_Composition)));
        }
    }

    [Test]
    public async Task
        _02_GraphWithEntry_AllInCollectionNavigations_FirstTrackedAsAggregation_ThenTrackedAsComposition_ThenTrackedAsAggregation_DoesNotThrow_AndUpdatesValuesCorrectly()
    {
        var entity = new Entity()
        {
            Text = "Init"
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();
        }

        var root = new RootNodeWithCollectionNavigations();
        root.A_Aggregations.Add((Entity)entity.Clone());
        root.B_Compositions.Add((Entity)entity.Clone());
        root.C_Aggregations.Add((Entity)entity.Clone());

        root.A_Aggregations[0].Text = nameof(root.A_Aggregations);
        root.B_Compositions[0].Text = nameof(root.B_Compositions);
        root.C_Aggregations[0].Text = nameof(root.C_Aggregations);
        
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(root));
            await dbContext.SaveChangesAsync();
        }
        
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var entityFromDb = await dbContext.Set<Entity>().SingleAsync(e => e.Id == entity.Id);
            
            Assert.That(entityFromDb.Text, Is.EqualTo(nameof(root.B_Compositions)));
        }
    }
}