using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.BugReports;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault;

public class EfDefaultBugReports : TestBase<EfDefaultBugReportsDbContext>
{
    [Test]
    public async Task _01_ConcurrencyTokenOfOwnedType_InTphConfiguration_ShouldBeSetAutomatically()
    {
        var init = new ConcreteTypeOne()
        {
            Owned = new()
        };

        await using (var dbContext = new EfDefaultBugReportsDbContext())
        {
            dbContext.Add(init);
            await dbContext.SaveChangesAsync();
        }
        
        Assert.That(init.Id, Is.GreaterThan(0));
        Assert.That(init.ConcurrencyToken, Is.GreaterThan(0));

        var update = new ConcreteTypeOne()
        {
            Id = init.Id,
            ConcurrencyToken = init.ConcurrencyToken,
            Owned = new()
        };
        
        await using (var dbContext = new EfDefaultBugReportsDbContext())
        {
            dbContext.Update(update);
            var conventionConcurrencyTokenCurrentValue = (uint)dbContext.Entry(update.Owned)
                .Property("_TableSharingConcurrencyTokenConvention_ConcurrencyToken").CurrentValue!;
            
            Assert.Multiple(() =>
            {
                // Should be equal to update.ConcurrencyToken
                Assert.That(conventionConcurrencyTokenCurrentValue, Is.Not.EqualTo(update.ConcurrencyToken)); 
                
                // Should not throw
                Assert.ThrowsAsync<InvalidOperationException>(async () => await dbContext.SaveChangesAsync()); 
            });
        }
        
        Assert.Warn("This is a bug in EF Core. See https://github.com/dotnet/efcore/issues/32720");
    }
}