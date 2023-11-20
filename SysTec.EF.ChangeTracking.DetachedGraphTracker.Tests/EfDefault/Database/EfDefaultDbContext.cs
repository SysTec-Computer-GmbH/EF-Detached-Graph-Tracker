using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.GraphTraversal;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Database;

public class EfDefaultDbContext : DbContextBase
{
    public DbSet<EfDefaultRootNode> EfDefaultRootNodes { get; set; }

    public DbSet<RootNodeWithRequiredSimpleList> RootNodesWithRequiredSimpleList { get; set; }

    public DbSet<RootNodeWithOptionalSimpleList> RootNodesWithOptionalSimpleList { get; set; }

    public DbSet<EfDefaultBaseType> EfDefaultBaseTypes { get; set; }

    public DbSet<EfDefaultItemWithOptionalRelationship> ItemsWithOptionalRelationship { get; set; }

    public DbSet<EfDefaultRootWithCollection> CollectionRoots { get; set; }

    public DbSet<EfDefaultRootWithOwnedType> RootsWithOwnedTypes { get; set; }

    public DbSet<EfDefaultItemWithRequiredRelationship> DefaultItemsWithRequiredRelationship { get; set; }

    public DbSet<EfDefaultRootWithReference> RootsWithReference { get; set; }

    public DbSet<EfDefaultManyItemOne> ManyItems { get; set; }

    public DbSet<GraphTraversalRootNode> GraphTraversalRootNodes { get; set; }

    public DbSet<EfDefaultManyRoot> ManyRoots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EfDefaultRootNode>()
            .HasMany(n => n.Items)
            .WithOne(i => i.EfDefaultRootNode);

        modelBuilder.Entity<RootNodeWithRequiredSimpleList>()
            .HasMany(rn => rn.ListItems)
            .WithOne()
            .IsRequired();

        modelBuilder.Entity<EfDefaultBaseType>()
            .HasDiscriminator(b => b.Discriminator)
            .HasValue<EfDefaultSubType>(nameof(EfDefaultSubType));

        modelBuilder.Entity<EfDefaultRelationshipManySide>()
            .HasOne(r => r.Relationship)
            .WithMany()
            .HasForeignKey(r => r.RelationshipId);

        modelBuilder.Entity<EfDefaultItemWithOptionalRelationship>()
            .HasOne(i => i.OptionalRelationship)
            .WithMany(i => i.Backreferences)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EfDefaultItemWithRequiredRelationship>()
            .HasOne(i => i.RequiredRelationship)
            .WithMany()
            .IsRequired();

        modelBuilder.Entity<EfDefaultOneItem>()
            .HasOne(i => i.Relationship)
            .WithOne(i2 => i2.Relationship)
            .HasForeignKey<EfDefaultOneItemTwo>()
            .IsRequired();

        modelBuilder.Entity<EfDefaultManyItemOne>()
            .HasMany(i => i.Items)
            .WithMany();

        modelBuilder.Entity<EfDefaultItemWithGeneratedValue>()
            .Property(i => i.GeneratedValue)
            .UseIdentityAlwaysColumn()
            .HasIdentityOptions(10);

        modelBuilder.Entity<EfDefaultItemWithUniqueConstraint>()
            .HasIndex(i => i.UniqueConstraintWhenTrue)
            .IsUnique()
            .HasFilter($"(\"{nameof(EfDefaultItemWithUniqueConstraint.UniqueConstraintWhenTrue)}\")=(TRUE)");
    }
}