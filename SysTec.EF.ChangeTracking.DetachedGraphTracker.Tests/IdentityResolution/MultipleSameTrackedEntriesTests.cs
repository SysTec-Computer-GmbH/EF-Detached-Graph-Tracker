using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleSameTrackedEntriesTests;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class MultipleSameTrackedEntriesTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task
        _01_GraphWithEntry_AllInReferenceNavigations_FirstTrackedAsAssociation_ThenTrackedAsComposition_ThenTrackedAsAssociation_DoesNotThrow_AndUpdatesValuesCorrectly()
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
            A_Association = (Entity)entity.Clone(),
            B_Composition = (Entity)entity.Clone(),
            C_Association = (Entity)entity.Clone()
        };
        
        root.A_Association.Text = nameof(root.A_Association);
        root.B_Composition.Text = nameof(root.B_Composition);
        root.C_Association.Text = nameof(root.C_Association);
        
        Assert.That(ReferenceEquals(root.A_Association, root.B_Composition), Is.False);
        
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
        _02_GraphWithEntry_AllInCollectionNavigations_FirstTrackedAsAssociation_ThenTrackedAsComposition_ThenTrackedAsAssociation_DoesNotThrow_AndUpdatesValuesCorrectly()
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
        root.A_Associations.Add((Entity)entity.Clone());
        root.B_Compositions.Add((Entity)entity.Clone());
        root.C_Associations.Add((Entity)entity.Clone());

        root.A_Associations[0].Text = nameof(root.A_Associations);
        root.B_Compositions[0].Text = nameof(root.B_Compositions);
        root.C_Associations[0].Text = nameof(root.C_Associations);
        
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