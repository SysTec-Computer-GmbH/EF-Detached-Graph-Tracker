using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Database;

public class PolymorphismTestsDbContext : DbContextBase
{
    public DbSet<RootEntityWithBaseTypeNavigations> RootEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseEntity>()
            .HasDiscriminator(be => be.Discriminator)
            .HasValue<SubEntityWithNormalKey>(nameof(SubEntityWithNormalKey))
            .HasValue<DifferentSubEntityWithNormalKey>(nameof(DifferentSubEntityWithNormalKey));
    }
}