using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Behavior;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association;

public class AssociationBehaviorTests : TestBase<AssociationTestsDbContext>
{
    [Test]
    public async Task _01_AddedEntityInAssociation_WithDefaultBehavior_ThrowsException()
    {
        var root = new RootWithDefaultThrowBehavior()
        {
            ItemWithDefaultThrowBehavior = new()
        };

        await using var dbContext = new AssociationTestsDbContext();
        var graphTracker = GetGraphTrackerInstance(dbContext);
        Assert.ThrowsAsync<AddedAssociationEntryException>(async () => await graphTracker.TrackGraphAsync(root));
    }

    [Test]
    public async Task _02_AddedEntityInAssociation_WithDetachBehavior_KeepsAddedAssociationItemDetached()
    {
        var root = new RootWithDetachBehavior()
        {
            ItemWithDetachBehavior = new()
        };

        await using var dbContext = new AssociationTestsDbContext();
        var graphTracker = GetGraphTrackerInstance(dbContext);
        Assert.DoesNotThrowAsync(async () => await graphTracker.TrackGraphAsync(root));
        Assert.That(dbContext.Entry(root.ItemWithDetachBehavior).State, Is.EqualTo(EntityState.Detached));
    }
}