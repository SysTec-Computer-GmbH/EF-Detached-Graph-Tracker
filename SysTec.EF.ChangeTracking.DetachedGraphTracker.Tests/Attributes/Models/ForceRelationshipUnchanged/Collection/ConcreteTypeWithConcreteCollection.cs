using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Collection;

public class ConcreteTypeWithConcreteCollection : AbstractTypeWithAbstractCollection
{
    [ForceKeepExistingRelationship]
    public override List<CollectionItemWithBackreferenceToAbstractType> Items { get; set; } = new();
}