using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class OptionalReferenceItemWithBackreference : IdBase
{
    [ForceKeepExistingRelationship]
    public List<RootWithOptionalReferenceWithFkWithBackreference> Roots { get; set; } = new();
}