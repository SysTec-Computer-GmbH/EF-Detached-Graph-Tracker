using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.ManyToMany;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToMany;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Models.OneToOne;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Relationships.Database;

public class RelationshipTestsDbContext : DbContextBase
{
    public DbSet<ManyEntityOne> ManyEntityOnes { get; set; }

    public DbSet<ManyEntityTwo> ManyEntityTwos { get; set; }

    public DbSet<SelfReferencingItem> SelfReferencingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DependentManyItemWithRequiredAggregation>()
            .HasOne(mi => mi.RequiredAggregation)
            .WithMany()
            .IsRequired();

        modelBuilder.Entity<DependentManyItemWithRequiredComposition>()
            .HasOne(mi => mi.RequiredComposition)
            .WithMany()
            .IsRequired();

        modelBuilder.Entity<DependentManyItemWithOptionalComposition>()
            .HasOne(mi => mi.OptionalComposition)
            .WithMany()
            .IsRequired(false);

        modelBuilder.Entity<DependentManyItemWithDefinedFkOptionalComposition>()
            .HasOne(mi => mi.OptionalComposition)
            .WithMany()
            .HasForeignKey(i => i.OptionalCompositionId)
            .IsRequired(false);

        modelBuilder.Entity<DependentManyItemWithOptionalAggregation>()
            .HasOne(mi => mi.OptionalAggregation)
            .WithMany()
            .IsRequired(false);

        modelBuilder.Entity<DependentManyItemWithDefinedFkOptionalAggregation>()
            .HasOne(mi => mi.OptionalAggregation)
            .WithMany()
            .HasForeignKey(mi => mi.OptionalAggregationId)
            .IsRequired(false);

        modelBuilder.Entity<DependentItemWithRequiredComposition>()
            .HasOne(i => i.RequiredPrincipal)
            .WithOne(pi => pi.OptionalDependent)
            .HasForeignKey<DependentItemWithRequiredComposition>()
            .IsRequired();

        modelBuilder.Entity<DependentItemWithRequiredAggregation>()
            .HasOne(i => i.RequiredPrincipal)
            .WithOne(pi => pi.OptionalDependent)
            .HasForeignKey<DependentItemWithRequiredAggregation>()
            .IsRequired();

        modelBuilder.Entity<RootNode>()
            .HasOne(r => r.B_Composition)
            .WithMany();

        modelBuilder.Entity<ManyEntityOne>()
            .HasMany(o => o.ManyCompositions)
            .WithMany(t => t.EntityOneAggregations);

        modelBuilder.Entity<ManyEntityOneAggregation>()
            .HasMany(o => o.ManyAggregations)
            .WithMany();
    }
}