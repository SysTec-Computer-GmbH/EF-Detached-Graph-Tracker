using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.WithIdentityResolution;

public class InvalidAttributeUsageTests : TestBase<ListTestsDbContext>
{
    [Test]
    public async Task _01_MultipleCompositions_OfSameTypeAndKeyValues_ThrowError_OnUpdate()
    {
        var rootNode = DataHelper.GetRootNodeWithMultipleCompositionsOfSameType();
        await Assert_SavingRootNode_DoesNotThrow(rootNode);

        var rootNodeUpdate = (RootNodeWithMultipleCompositionsOfSameType)rootNode.Clone();
        rootNodeUpdate.CompositionItems[0].Id = rootNode.CompositionItem.Id;
        Assert_SavingUpdatedRootNode_Throws(rootNodeUpdate);
    }

    [Test]
    public async Task _02_MultipleNestedCompositions_OfSameTypeAndKeyValues_ThrowError_OnUpdate()
    {
        var rootNode = DataHelper.GetRootNodeWithMultipleNestedCompositionsOfSameType();
        await Assert_SavingRootNode_DoesNotThrow(rootNode);

        var rootNodeUpdate = (RootNodeWithMultipleNestedCompositionsOfSameType)rootNode.Clone();
        rootNodeUpdate.A_ExtraLayerItem.CompositionItems[0].Id = rootNode.B_CompositionItems[0].Id;
        Assert_SavingUpdatedRootNode_Throws(rootNodeUpdate);
    }

    private async Task Assert_SavingRootNode_DoesNotThrow(object rootNode)
    {
        await using var dbContext = new ListTestsDbContext();
        // When saving for the first time, with all items in the added state,
        // there is no easy way to find out if there are forbidden multiple compositions in the graph
        var graphTracker = GetGraphTrackerInstance(dbContext);
        Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(rootNode));
        await dbContext.SaveChangesAsync();
    }

    private void Assert_SavingUpdatedRootNode_Throws(object rootNodeUpdate)
    {
        using var dbContext = new ListTestsDbContext();
        var graphTracker = GetGraphTrackerInstance(dbContext); 
        Assert.ThrowsAsync<MultipleCompositionException>(async () =>
            await graphTracker.TrackGraphAsync(rootNodeUpdate));
    }
}