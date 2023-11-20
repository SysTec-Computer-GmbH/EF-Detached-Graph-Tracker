using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Limitations.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Limitations.Database;

public class LimitationTestDbContext : DbContextBase
{
    public DbSet<NodeWithoutKey> NodesWithoutKeys { get; set; }

    public DbSet<NodeWithInvalidKeyType> NodesWithInvalidKeyTypes { get; set; }
}