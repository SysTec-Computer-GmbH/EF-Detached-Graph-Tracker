using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Behavior;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation;

public class ForceAggregationBehaviorTests : TestBase<ForceAggregationTestsDbContext>
{
    [Test]
    public async Task _01_AddedEntityInForceAggregation_WithDefaultBehavior_ThrowsException()
    {
        var root = new RootWithDefaultThrowBehavior()
        {
            ItemWithDefaultThrowBehavior = new()
        };

        await using var dbContext = new ForceAggregationTestsDbContext();
        var graphTracker = GetGraphTrackerInstance(dbContext);
        Assert.ThrowsAsync<AddedAssociationEntryException>(async () => await graphTracker.TrackGraphAsync(root));
    }

    [Test]
    public async Task _02_AddedEntityInForceAggregation_WithDetachBehavior_KeepsAddedAggregationItemDetached()
    {
        var root = new RootWithDetachBehavior()
        {
            ItemWithDetachBehavior = new()
        };

        await using var dbContext = new ForceAggregationTestsDbContext();
        var graphTracker = GetGraphTrackerInstance(dbContext);
        Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(root));
        Assert.That(dbContext.Entry(root.ItemWithDetachBehavior).State, Is.EqualTo(EntityState.Detached));
    }
}