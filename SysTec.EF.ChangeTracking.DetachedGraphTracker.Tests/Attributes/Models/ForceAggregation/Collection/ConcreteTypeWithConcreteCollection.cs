using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceAggregation.Collection;

public class ConcreteTypeWithConcreteCollection : AbstractTypeWithAbstractCollection
{
    [ForceAggregation]
    public override List<CollectionItemWithBackreferenceToAbstractType> Items { get; set; } = new();
}