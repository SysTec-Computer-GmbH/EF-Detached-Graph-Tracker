using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Limitations.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Limitations.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Limitations;

public class LimitationTests : TestBase<LimitationTestDbContext>
{
    [Test]
    public void _01_TrackingNode_WithNullValueInPrimaryKey_Throws()
    {
        var nodeWithoutKey = new NodeWithoutKey();

        using var dbContext = new LimitationTestDbContext();
        var graphTracker = GetGraphTrackerInstance(dbContext);
        Assert.ThrowsAsync<NoPrimaryKeyException>(async () =>
             await graphTracker.TrackGraphAsync(nodeWithoutKey));
    }
}