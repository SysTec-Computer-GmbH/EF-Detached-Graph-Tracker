using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ConcurrencyStamps.Database;

public class ConcurrencyStampTestDbContext : DbContextBase
{
    public DbSet<ItemWithConcurrencyStamp> Items { get; set; }

    public DbSet<RootWithConcurrencyItemInAssociationReference> Roots { get; set; }
}