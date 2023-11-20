using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.SimpleGraph;

public class ListDeleteTests : TestBase<ListTestsDbContext>
{
    [Test]
    public async Task _01_01_WithoutForceDelete_RequiredListItems_AreDeletedFromDb()
    {
        var rootNode = DataHelper.GetRootNodeWithRequiredSimpleList();

        await using (var dbContext = new ListTestsDbContext())
        {
            // EF default add method is used because other tests test create using TrackGraph
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithRequiredSimpleList)rootNode.Clone();
        // Remove the first and third item
        rootNodeUpdate.ListItems.RemoveAll(li => li.Text != "Item2");

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithRequiredSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(1));
            Assert.That(rootNodeFromDb.ListItems[0].Text, Is.EqualTo("Item2"));
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Proof, that the deleted items are also deleted from the database, when the relationship is required
            var listItems = await dbContext.Set<RequiredListItem>().ToListAsync();
            Assert.That(listItems, Has.Count.EqualTo(1));
        }
    }

    [Test]
    public async Task _01_02_WithoutForceDelete_RequiredListItems_AreDeletedFromDb_WhenListIsNull()
    {
        var rootNode = DataHelper.GetRootNodeWithRequiredSimpleList();

        await using (var dbContext = new ListTestsDbContext())
        {
            // EF default add method is used because other tests test create using TrackGraph
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithRequiredSimpleList)rootNode.Clone();
        rootNodeUpdate.ListItems = null;

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithRequiredSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.That(rootNodeFromDb.ListItems, Is.Empty);
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Proof, that the deleted items are also deleted from the database, when the relationship is required
            var listItems = await dbContext.Set<RequiredListItem>().ToListAsync();
            Assert.That(listItems, Is.Empty);
        }
    }

    [Test]
    public async Task _02_01_WithoutForceDelete_OptionalListItems_AreNotDeletedFromDb()
    {
        var rootNode = DataHelper.GetRootNodeWithOptionalSimpleList();

        await using (var dbContext = new ListTestsDbContext())
        {
            // EF default add method is used because other tests test create using TrackGraph
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithOptionalSimpleList)rootNode.Clone();
        // Remove the first and third item
        rootNodeUpdate.ListItems.RemoveAll(li => li.Text != "Item2");

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithOptionalSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(1));
            Assert.That(rootNodeFromDb.ListItems[0].Text, Is.EqualTo("Item2"));
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Proof, that the deleted items are not deleted from the database, when the relationship is optional
            var listItems = await dbContext.Set<OptionalListItem>().ToListAsync();
            Assert.That(listItems, Has.Count.EqualTo(3));
        }
    }

    [Test]
    public async Task _02_02_WithoutForceDelete_OptionalListItems_AreNotDeletedFromDb_WhenListIsNull()
    {
        var rootNode = DataHelper.GetRootNodeWithOptionalSimpleList();

        await using (var dbContext = new ListTestsDbContext())
        {
            // EF default add method is used because other tests test create using TrackGraph
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithOptionalSimpleList)rootNode.Clone();
        rootNodeUpdate.ListItems = null;

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithOptionalSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.That(rootNodeFromDb.ListItems, Is.Empty);
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Proof, that the deleted items are not deleted from the database, when the relationship is optional
            var listItems = await dbContext.Set<OptionalListItem>().ToListAsync();
            Assert.That(listItems, Has.Count.EqualTo(3));
        }
    }

    [Test]
    public async Task _03_01_WithForceDelete_OptionalListItems_AreDeletedFromDb()
    {
        var rootNode = DataHelper.GetRootNodeWithOptionalSimpleListAndForceDelete();

        await using (var dbContext = new ListTestsDbContext())
        {
            // EF default add method is used because other tests test create using TrackGraph
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithOptionalSimpleListAndForceDelete)rootNode.Clone();
        // Remove the first and third item
        rootNodeUpdate.ListItems.RemoveAll(li => li.Text != "Item2");

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb =
                await DataHelper.GetRootNodeWithOptionalSimpleListAndForceDeleteAttributeFromDbWithIncludesAsync(
                    dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(1));
            Assert.That(rootNodeFromDb.ListItems[0].Text, Is.EqualTo("Item2"));
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Proof, that the deleted items are deleted from the database, when the relationship is optional but ForceDelete is set
            var listItems = await dbContext.Set<OptionalListItem>().ToListAsync();
            Assert.That(listItems, Has.Count.EqualTo(1));
        }
    }

    [Test]
    public async Task _03_02_WithForceDelete_OptionalListItems_AreDeletedFromDb_WhenListIsNull()
    {
        var rootNode = DataHelper.GetRootNodeWithOptionalSimpleListAndForceDelete();

        await using (var dbContext = new ListTestsDbContext())
        {
            // EF default add method is used because other tests test create using TrackGraph
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithOptionalSimpleListAndForceDelete)rootNode.Clone();
        rootNodeUpdate.ListItems = null;

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb =
                await DataHelper.GetRootNodeWithOptionalSimpleListAndForceDeleteAttributeFromDbWithIncludesAsync(
                    dbContext);
            Assert.That(rootNodeFromDb.ListItems, Is.Empty);
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Proof, that the deleted items are deleted from the database, when the relationship is optional but ForceDelete is set
            var listItems = await dbContext.Set<OptionalListItem>().ToListAsync();
            Assert.That(listItems, Is.Empty);
        }
    }
}