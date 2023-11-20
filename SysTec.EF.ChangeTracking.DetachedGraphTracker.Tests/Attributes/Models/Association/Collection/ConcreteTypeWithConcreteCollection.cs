using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.Association.Collection;

public class ConcreteTypeWithConcreteCollection : AbstractTypeWithAbstractCollection
{
    [UpdateAssociationOnly]
    public override List<CollectionItemWithBackreferenceToAbstractType> Items { get; set; } = new();
}