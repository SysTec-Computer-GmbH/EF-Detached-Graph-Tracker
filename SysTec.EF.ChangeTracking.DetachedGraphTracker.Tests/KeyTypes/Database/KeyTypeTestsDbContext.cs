using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.KeyTypes.Database;

public class KeyTypeTestsDbContext : DbContextBase
{
    public DbSet<EntityWithIntKey> EntityWithIntKeys { get; set; }
    
    public DbSet<EntityWithLongKey> EntityWithLongKeys { get; set; }
    
    public DbSet<EntityWithStringKey> EntityWithStringKeys { get; set; }
    
    public DbSet<EntityWithGuidKey> EntityWithGuidKeys { get; set; }

    public DbSet<EntityWithByteArrayKey> EntityWithByteArrayKeys { get; set; }
}