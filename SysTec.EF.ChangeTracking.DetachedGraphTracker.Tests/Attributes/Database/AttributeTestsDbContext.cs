using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.Association.Collection;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Reference;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Database;

public class AttributeTestsDbContext : DbContextBase
{
    public DbSet<ConcreteTypeWithConcreteCollection> AssociationConcreteCollectionTypes { get; set; }
    
    public DbSet<Models.ForceDelete.Collection.ConcreteTypeWithConcreteCollection> ForceDeleteConcreteTypes { get; set; }
    
    public DbSet<Models.ForceRelationshipUnchanged.Collection.ConcreteTypeWithConcreteCollection> ForceRelationshipUnchangedConcreteTypes { get; set; }
    
    public DbSet<ConcreteTypeWithConcreteReference> ForceRelationshipUnchangedConcreteReferenceTypes { get; set; }
    
    public DbSet<Models.Association.Reference.ConcreteTypeWithConcreteReference> AssociationConcreteReferenceTypes { get; set; }
}