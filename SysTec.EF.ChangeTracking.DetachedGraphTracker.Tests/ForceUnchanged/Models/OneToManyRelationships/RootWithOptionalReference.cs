using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class RootWithOptionalReference : IdBase
{
    public int? OptionalItemId { get; set; }
    
    [ForceKeepExistingRelationship]
    public OptionalReferenceItem? OptionalItem { get; set; }
}