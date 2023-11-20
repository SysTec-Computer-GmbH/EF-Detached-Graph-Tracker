using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.CompositeKeys.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.CompositeKeys.Database;

public class CompositeKeyTestsDbContext : DbContextBase
{
    public DbSet<CompositeKeyEntity> CompositeKeyEntities { get; set; }
}