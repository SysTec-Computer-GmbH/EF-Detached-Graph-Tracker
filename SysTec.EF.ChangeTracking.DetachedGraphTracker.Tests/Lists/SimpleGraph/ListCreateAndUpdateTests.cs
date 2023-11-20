using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.SimpleGraph;

public class ListCreateAndUpdateTests : TestBase<ListTestsDbContext>
{
    [Test]
    public async Task _01_WithoutAssociation_TestCreateWithSimpleList()
    {
        var rootNode = DataHelper.GetRootNodeWithRequiredSimpleList();

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithRequiredSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(3));
        }
    }

    [Test]
    public async Task _02_WithoutAssociation_TestUpdateWithSimpleList()
    {
        var rootNode = DataHelper.GetRootNodeWithRequiredSimpleList();

        await using (var dbContext = new ListTestsDbContext())
        {
            // EF default add method is used because test _01 already tests create using TrackGraph
            dbContext.Add(rootNode);
            await dbContext.SaveChangesAsync();
        }

        var rootNodeUpdate = (RootNodeWithRequiredSimpleList)rootNode.Clone();
        const string updatedText1 = "Item1Updated";
        const string updatedText2 = "Item2Updated";
        const string updatedText3 = "Item3Updated";
        rootNodeUpdate.ListItems[0].Text = updatedText1;
        rootNodeUpdate.ListItems[1].Text = updatedText2;
        rootNodeUpdate.ListItems[2].Text = updatedText3;

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNodeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb = await DataHelper.GetRootNodeWithRequiredSimpleListFromDbWithIncludesAsync(dbContext);
            Assert.Multiple(() =>
            {
                Assert.That(rootNodeFromDb.ListItems[0].Text, Is.EqualTo(updatedText1));
                Assert.That(rootNodeFromDb.ListItems[1].Text, Is.EqualTo(updatedText2));
                Assert.That(rootNodeFromDb.ListItems[2].Text, Is.EqualTo(updatedText3));
            });
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Assert that update doesn't create new items
            var listItemsFromDbCount = await dbContext.Set<RequiredListItem>().CountAsync();
            Assert.That(listItemsFromDbCount, Is.EqualTo(3));
        }
    }

    [Test]
    public async Task _03_WithAssociation_TestCreateWithSimpleList()
    {
        var rootNode = DataHelper.GetRootNodeWithOptionalSimpleListAndAssociation();

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); 
            Assert.ThrowsAsync<AddedAssociationEntryException>(async () => await graphTracker.TrackGraphAsync(rootNode)); 
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb =
                await DataHelper.GetRootNodeWithOptionalSimpleListAndAssociationAttributeFromDbWithIncludesAsync(
                    dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(0));
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            // Assure that nothing gets created in the database when list items without a key are provided in a force association
            var hasSavedListItems = await dbContext.Set<OptionalListItem>().AnyAsync();
            Assert.That(hasSavedListItems, Is.False);
        }
    }

    [Test]
    public async Task _04_WithAssociation_TestRelationshipIsCreated_AndItemsAreNotModified()
    {
        var optionalListItems = DataHelper.GetOptionalListItems();

        await using (var dbContext = new ListTestsDbContext())
        {
            dbContext.AddRange(optionalListItems);
            await dbContext.SaveChangesAsync();
        }

        var rootNode = DataHelper.GetRootNodeWithOptionalSimpleListAndAssociation();
        var clonedOptionalListItems = optionalListItems.Select(x => (OptionalListItem)x.Clone()).ToList();
        clonedOptionalListItems[0].Text = "Item1Update";
        clonedOptionalListItems[1].Text = "Item2Update";
        clonedOptionalListItems[2].Text = "Item3Update";
        rootNode.ListItems = clonedOptionalListItems;

        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext); await graphTracker.TrackGraphAsync(rootNode);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var rootNodeFromDb =
                await DataHelper.GetRootNodeWithOptionalSimpleListAndAssociationAttributeFromDbWithIncludesAsync(
                    dbContext);
            Assert.That(rootNodeFromDb.ListItems, Has.Count.EqualTo(3));
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var listItemsFromDb = await dbContext.Set<OptionalListItem>().ToListAsync();
            Assert.Multiple(() =>
            {
                Assert.That(listItemsFromDb[0].Text, Is.EqualTo("Item1"));
                Assert.That(listItemsFromDb[1].Text, Is.EqualTo("Item2"));
                Assert.That(listItemsFromDb[2].Text, Is.EqualTo("Item3"));
            });
        }
    }
}