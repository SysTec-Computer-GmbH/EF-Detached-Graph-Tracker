using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Realistic.Database;

public class TestDbContext : DbContextBase
{
    public DbSet<Offer> Offers { get; set; }

    public DbSet<OfferPosition> Positions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OfferPosition>()
            .UseTphMappingStrategy()
            .HasDiscriminator(p => p.Discriminator)
            .HasValue<HeaderPosition>(nameof(HeaderPosition))
            .HasValue<BillablePosition>(nameof(BillablePosition));

        modelBuilder.Entity<OfferPosition>()
            .HasOne<Offer>()
            .WithMany(a => a.Positions)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder.Entity<OfferPosition>()
            .HasMany(p => p.Children)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectSectionPermit>()
            .HasMany(va => va.Sections)
            .WithMany();
    }
}