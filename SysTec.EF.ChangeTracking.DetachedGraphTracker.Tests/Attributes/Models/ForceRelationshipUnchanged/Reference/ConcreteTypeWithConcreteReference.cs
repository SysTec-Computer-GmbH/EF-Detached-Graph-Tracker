using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Reference;

public class ConcreteTypeWithConcreteReference : AbstractTypeWithAbstractReference
{
    public override int? ItemId { get; set; }
    
    [ForceKeepExistingRelationship]
    public override ReferenceItemWithBackreferenceToAbstractType Item { get; set; } 
}