using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class RootWithRequiredReference : IdBase
{
    public int RequiredItemId { get; set; }
    
    [ForceKeepExistingRelationship]
    public RequiredReferenceItem RequiredItem { get; set; }
}