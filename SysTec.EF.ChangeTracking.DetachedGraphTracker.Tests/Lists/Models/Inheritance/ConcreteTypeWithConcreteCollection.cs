using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Inheritance;

public class ConcreteTypeWithConcreteCollection : BaseTypeWithAbstractCollection
{
    [ForceDeleteOnMissingEntries]
    public override List<OptionalListItemWithBackreferenceToBaseType> Items { get; set; } = new();
}