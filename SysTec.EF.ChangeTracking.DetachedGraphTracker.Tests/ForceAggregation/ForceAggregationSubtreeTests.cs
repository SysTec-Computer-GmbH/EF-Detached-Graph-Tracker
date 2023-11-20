using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Subtree;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association;

public class AssociationSubtreeTests : TestBase<AssociationTestsDbContext>
{
    [Test]
    public async Task _01_RelationshipInsideExistingAssociation_WithinReferenceNavigation_CanNotBeModified()
    {
        var root = new RootNode
        {
            Association = new AssociationRoot
            {
                Text = "Association"
            }
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (RootNode)root.Clone();
        rootUpdate.Association!.Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Association)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Association!.Composition, Is.Null); });
        }

        var associationUpdate = (AssociationRoot)root.Association.Clone();
        associationUpdate.Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(associationUpdate);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate2 = (RootNode)root.Clone();
        rootUpdate2.Association = (AssociationRoot)associationUpdate.Clone();
        rootUpdate2.Association.Composition = null;

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Association)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Association!.Composition, Is.Not.Null); });
        }
    }

    [Test]
    public async Task _02_RelationshipInsideExistingAssociation_WithinCollectionNavigation_CanNotBeModified()
    {
        var root = new RootNode
        {
            Associations = new List<AssociationRoot>
            {
                new AssociationRoot
                {
                    Text = "Association"
                }
            }
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            dbContext.Add(root);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate = (RootNode)root.Clone();
        rootUpdate.Associations[0].Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Associations)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Associations[0].Composition, Is.Null); });
        }

        var associationUpdate = (AssociationRoot)root.Associations[0].Clone();
        associationUpdate.Composition = new Entity
        {
            Text = "Composition"
        };

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(associationUpdate);
            await dbContext.SaveChangesAsync();
        }

        var rootUpdate2 = (RootNode)root.Clone();
        rootUpdate2.Associations.Clear();
        rootUpdate2.Associations.Add((AssociationRoot)associationUpdate.Clone());
        rootUpdate2.Associations[0].Composition = null;

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootUpdate2);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new AssociationTestsDbContext())
        {
            var rootFromDb = await dbContext.Set<RootNode>()
                .Include(r => r.Associations)
                .ThenInclude(a => a.Composition)
                .SingleAsync(x => x.Id == root.Id);

            Assert.Multiple(() => { Assert.That(rootFromDb.Associations[0].Composition, Is.Not.Null); });
        }
    }
}