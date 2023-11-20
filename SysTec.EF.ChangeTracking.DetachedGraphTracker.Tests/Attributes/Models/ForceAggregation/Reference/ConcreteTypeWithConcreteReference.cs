using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceAggregation.Reference;

public class ConcreteTypeWithConcreteReference : AbstractTypeWithAbstractReference
{
    public override int? ItemId { get; set; }
    
    [UpdateAssociationOnly]
    public override ReferenceItemWithBackreferenceToAbstractType Item { get; set; } 
}