using Microsoft.EntityFrameworkCore;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests;

public class TestBase<TDbContext> where TDbContext : DbContext, new()
{
    [SetUp]
    public async Task SetUp()
    {
        var dbContext = new TDbContext();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
    
    protected DetachedGraphTracker GetGraphTrackerInstance(DbContext dbContext)
    {
        return new DetachedGraphTracker(dbContext);
    }
}