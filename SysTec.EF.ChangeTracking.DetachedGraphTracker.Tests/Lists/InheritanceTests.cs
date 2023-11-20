using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Inheritance;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists;

public class InheritanceTests : TestBase<ListTestsDbContext>
{
    [Test]
    public async Task _01_WithForceDelete_OptionalListItems_AreDeletedFromDb_WhenRelationshipIsConfigured_ToBaseType()
    {
        var concreteType = new ConcreteTypeWithConcreteCollection()
        {
            Items = new()
            {
                new()
            }
        };
        
        await using (var dbContext = new ListTestsDbContext())
        {
            dbContext.Add(concreteType);
            await dbContext.SaveChangesAsync();
        }

        var concreteTypeUpdate = new ConcreteTypeWithConcreteCollection()
        {
            Id = concreteType.Id,
            Items = new()
        };
        
        await using (var dbContext = new ListTestsDbContext())
        {
            var graphTracker = GetGraphTrackerInstance(dbContext);
            await graphTracker.TrackGraphAsync(concreteTypeUpdate);
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var concreteTypeFromDb = await dbContext.Set<BaseTypeWithAbstractCollection>().SingleAsync(o => o.Id == concreteType.Id);
            Assert.That(concreteTypeFromDb.Items, Is.Empty);
        }

        await using (var dbContext = new ListTestsDbContext())
        {
            var listItems = await dbContext.Set<OptionalListItemWithBackreferenceToBaseType>().ToListAsync();
            Assert.That(listItems, Is.Empty);
        }
    }
}