using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.WithIdentityResolution;

public class CustomCollectionLoadTests : TestBase<ListTestsDbContext>
{
    [Test]
    public async Task
        _01_CompositionInChangeTracker_IsNotOverwritten_WithDatabaseValue_WhenAggregationOfSameTypeAndKeyIsLoaded_ForRemovingItFromAnAggregationCollection()
    {
        var aggregationItem = new Item
        {
            Text = "AggregationItem"
        };

        await using (var dbContext = new ListTestsDbContext())
        {
            dbContext.Add(aggregationItem);
            await dbContext.SaveChangesAsync();
        }

        var rootNode = new RootNodeWithCompositionAndAggregationOfSameType
        {
            A_Composition_Item = aggregationItem,
            B_Aggregation_Items = { aggregationItem }
        };

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.RootNodesWithCompositionAndAggregationOfSameType
                .Include(x => x.A_Composition_Item)
                .Include(x => x.B_Aggregation_Items)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.A_Composition_Item, Is.Not.Null);
                Assert.That(rootNodeFromDb.B_Aggregation_Items, Has.Count.EqualTo(1));
            });
        }

        const string updatedText = "UpdatedText";
        var rootNodeUpdate = (RootNodeWithCompositionAndAggregationOfSameType)rootNode.Clone();
        rootNodeUpdate.A_Composition_Item.Text = updatedText;
        rootNodeUpdate.B_Aggregation_Items.Clear();

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await dbContext.RootNodesWithCompositionAndAggregationOfSameType
                .Include(x => x.A_Composition_Item)
                .Include(x => x.B_Aggregation_Items)
                .SingleAsync(x => x.Id == rootNode.Id);

            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.A_Composition_Item.Text, Is.EqualTo(updatedText));
                Assert.That(rootNodeFromDb.B_Aggregation_Items, Has.Count.EqualTo(0));
            });
        }
    }
}