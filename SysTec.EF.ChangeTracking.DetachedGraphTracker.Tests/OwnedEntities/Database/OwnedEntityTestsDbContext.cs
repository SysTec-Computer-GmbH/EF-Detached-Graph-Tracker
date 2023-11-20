using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Database;

public class OwnedEntityTestsDbContext : DbContextBase
{
    public DbSet<SubType_A> SubTypeAs { get; set; }

    public DbSet<SubType_B> SubTypeBs { get; set; }

    public DbSet<TypeWithOwnedCollection> TypeWithOwnedCollections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseType>()
            .HasDiscriminator(b => b.Discriminator)
            .HasValue<SubType_A>(nameof(SubType_A))
            .HasValue<SubType_B>(nameof(SubType_B));
    }
}