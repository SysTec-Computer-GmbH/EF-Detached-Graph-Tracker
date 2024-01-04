using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class OwnedType
{
    [ForceKeepExistingRelationship]
    public OptionalReferenceItem OptionalItem { get; set; }
}