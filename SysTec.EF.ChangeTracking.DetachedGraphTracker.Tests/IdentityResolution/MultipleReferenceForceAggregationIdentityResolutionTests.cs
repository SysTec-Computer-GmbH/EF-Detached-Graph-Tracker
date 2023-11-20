using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleAssociation;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public class MultipleReferenceAssociationIdentityResolutionTests : TestBase<IdentityResolutionTestsDbContext>
{
    [Test]
    public async Task
        _01_IdentityResolution_ForMultipleReferenceAssociationsInGraph_ShouldNotAffectAssociationBehavior()
    {
        var forceAssociationItem = await CreateForeAssociationItem();

        var associationClone1 = (AssociationItem)forceAssociationItem.Clone();
        associationClone1.Text = "1ShouldNotBePersisted";

        var associationClone2 = (AssociationItem)forceAssociationItem.Clone();
        associationClone2.Text = "2ShouldNotBePersisted";

        var root = new MultiAssociationRoot
        {
            Text = "AssociationRoot",
            AssociationItem = associationClone1,
            CompositionItem = new CompositionItem
            {
                Text = "CompositionItem",
                AssociationItem = associationClone2
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        MultiAssociationRoot clonedRootFromDb;
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await GetAssociationRootFromDb(dbContext, root.Id);

            clonedRootFromDb = (MultiAssociationRoot)rootFromDb.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.AssociationItem, Is.Not.Null);
                Assert.That(rootFromDb.AssociationItem!.Id, Is.EqualTo(forceAssociationItem.Id));
                Assert.That(rootFromDb.AssociationItem.Text, Is.EqualTo(forceAssociationItem.Text));
            });

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.CompositionItem.AssociationItem.Id, Is.EqualTo(forceAssociationItem.Id));
                Assert.That(rootFromDb.CompositionItem.AssociationItem.Text, Is.EqualTo(forceAssociationItem.Text));
            });
        }

        clonedRootFromDb.CompositionItem.AssociationItem = null;

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(clonedRootFromDb);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeFromDb = await GetAssociationRootFromDb(dbContext, clonedRootFromDb.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.AssociationItem, Is.Not.Null);
                Assert.That(rootNodeFromDb.AssociationItem!.Id, Is.EqualTo(forceAssociationItem.Id));
                Assert.That(rootNodeFromDb.AssociationItem.Text, Is.EqualTo(forceAssociationItem.Text));
            });

            Assert.That(rootNodeFromDb.CompositionItem.AssociationItem, Is.Null);
        }
    }

    [Test]
    public async Task
        _02_IdentityResolution_ForMultipleCollectionAssociationsInGraph_ShouldNotAffectAssociationBehavior()
    {
        var forceAssociationItem = await CreateForeAssociationItem();

        var associationClone1 = (AssociationItem)forceAssociationItem.Clone();
        associationClone1.Text = "1ShouldNotBePersisted";

        var associationClone2 = (AssociationItem)forceAssociationItem.Clone();
        associationClone2.Text = "2ShouldNotBePersisted";

        var root = new MultiAssociationRoot
        {
            Text = "AssociationRoot",
            AssociationItems = { associationClone1 },
            CompositionItem = new CompositionItem
            {
                Text = "CompositionItem",
                AssociationItems = { associationClone2 }
            }
        };

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(root);
            await dbContext.SaveChangesAsync();
        }

        MultiAssociationRoot clonedRootFromDb;
        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootFromDb = await GetAssociationRootFromDb(dbContext, root.Id);

            clonedRootFromDb = (MultiAssociationRoot)rootFromDb.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.AssociationItems, Has.Count.EqualTo(1));
                Assert.That(rootFromDb.AssociationItems[0].Id, Is.EqualTo(forceAssociationItem.Id));
                Assert.That(rootFromDb.AssociationItems[0].Text, Is.EqualTo(forceAssociationItem.Text));
            });

            Assert.Multiple(() =>
            {
                Assert.That(rootFromDb.CompositionItem.AssociationItems, Has.Count.EqualTo(1));
                Assert.That(rootFromDb.CompositionItem.AssociationItems[0].Id, Is.EqualTo(forceAssociationItem.Id));
                Assert.That(rootFromDb.CompositionItem.AssociationItems[0].Text, Is.EqualTo(forceAssociationItem.Text));
            });
        }

        clonedRootFromDb.CompositionItem.AssociationItems.Clear();

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(clonedRootFromDb);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new IdentityResolutionTestsDbContext())
        {
            var rootNodeFromDb = await GetAssociationRootFromDb(dbContext, clonedRootFromDb.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.AssociationItems, Has.Count.EqualTo(1));
                Assert.That(rootNodeFromDb.AssociationItems[0].Id, Is.EqualTo(forceAssociationItem.Id));
                Assert.That(rootNodeFromDb.AssociationItems[0].Text, Is.EqualTo(forceAssociationItem.Text));
            });

            Assert.That(rootNodeFromDb.CompositionItem.AssociationItems, Is.Empty);
        }
    }

    private async Task<AssociationItem> CreateForeAssociationItem()
    {
        var forceAssociationItem = new AssociationItem
        {
            Text = "AssociationItem"
        };

        await using var dbContext = new IdentityResolutionTestsDbContext();
        dbContext.Add(forceAssociationItem);
        await dbContext.SaveChangesAsync();

        return forceAssociationItem;
    }

    private async Task<MultiAssociationRoot> GetAssociationRootFromDb(
        IdentityResolutionTestsDbContext dbContext, int id)
    {
        return await dbContext.MultiAssociationRoots
            .Include(rn => rn.AssociationItems)
            .Include(rn => rn.AssociationItem)
            .Include(rn => rn.CompositionItem.AssociationItem)
            .Include(rn => rn.CompositionItem.AssociationItems)
            .SingleAsync(rn => rn.Id == id);
    }
}