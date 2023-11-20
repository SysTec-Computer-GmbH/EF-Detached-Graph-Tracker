using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.ManyToManyRelationships;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToOneRelationship;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Database;

public class ForceUnchangedDbContext : DbContextBase
{
    public DbSet<RootWithRequiredReference> RootsWithRequiredReferences { get; set; }
    
    public DbSet<RootWithOptionalReference> RootsWithOptionalReferences { get; set; }

    public DbSet<RootWithOptionalReferenceWithFkWithBackreference> ReferenceRootsWithFkAndBackreferences { get; set; }
    
    public DbSet<RootWithOptionalCollectionWithFkWithBackreference> CollectionRootsWithFkAndBackreferences { get; set; }

    public DbSet<ManyItemOne> ManyItemOnes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RootWithRequiredCollection>()
            .HasMany(r => r.RequiredItems)
            .WithOne()
            .HasForeignKey(i => i.RootId)
            .IsRequired();

        modelBuilder.Entity<RootWithOptionalCollection>()
            .HasMany(r => r.OptionalItems)
            .WithOne()
            .HasForeignKey(i => i.RootId)
            .IsRequired(false);

        modelBuilder.Entity<OptionalOneItemOne>()
            .HasOne(io => io.OptionalItem)
            .WithOne(oi => oi.OptionalItem)
            .HasForeignKey<OptionalOneItemTwo>(oi => oi.OptionalItemId)
            .IsRequired(false);
    }
}