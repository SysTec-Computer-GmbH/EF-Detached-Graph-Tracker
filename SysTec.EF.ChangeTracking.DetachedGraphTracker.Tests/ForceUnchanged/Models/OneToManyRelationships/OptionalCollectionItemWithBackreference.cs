using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class OptionalCollectionItemWithBackreference : IdBase
{
    public int? RootId { get; set; }
    
    [ForceKeepExistingRelationship]
    public RootWithOptionalCollectionWithFkWithBackreference Root { get; set; }
}