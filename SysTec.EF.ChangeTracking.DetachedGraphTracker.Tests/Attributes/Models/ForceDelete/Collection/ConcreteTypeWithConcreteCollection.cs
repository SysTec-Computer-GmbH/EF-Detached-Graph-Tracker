using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceDelete.Collection;

public class ConcreteTypeWithConcreteCollection : AbstractTypeWithAbstractCollection
{
    [ForceDeleteOnMissingEntries]
    public override List<CollectionItemWithBackreferenceToAbstractType> Items { get; set; } = new();
}