using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.WithIdentityResolution;

public class CustomCollectionLoadTests : TestBase<ListTestsDbContext>
{
    [Test]
    public async Task
        _01_CompositionInChangeTracker_IsNotOverwritten_WithDatabaseValue_WhenAssociationOfSameTypeAndKeyIsLoaded_ForRemovingItFromAnAssociationCollection()
    {
        var associationItem = new Item
        {
            Text = "AssociationItem"
        };

        await using (var dbContext = new ListTestsDbContext())
        {
            dbContext.Add(associationItem);
            await dbContext.SaveChangesAsync();
        }

        var rootNode = new RootNodeWithCompositionAndAssociationOfSameType
        {
            A_Composition_Item = associationItem,
            B_Association_Items = { associationItem }
        };

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.RootNodesWithCompositionAndAssociationOfSameType
                .Include(x => x.A_Composition_Item)
                .Include(x => x.B_Association_Items)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.A_Composition_Item, Is.Not.Null);
                Assert.That(rootNodeFromDb.B_Association_Items, Has.Count.EqualTo(1));
            });
        }

        const string updatedText = "UpdatedText";
        var rootNodeUpdate = (RootNodeWithCompositionAndAssociationOfSameType)rootNode.Clone();
        rootNodeUpdate.A_Composition_Item.Text = updatedText;
        rootNodeUpdate.B_Association_Items.Clear();

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.RootNodesWithCompositionAndAssociationOfSameType
                .Include(x => x.A_Composition_Item)
                .Include(x => x.B_Association_Items)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.A_Composition_Item.Text, Is.EqualTo(updatedText));
                Assert.That(rootNodeFromDb.B_Association_Items, Has.Count.EqualTo(0));
            });
        }
    }
}