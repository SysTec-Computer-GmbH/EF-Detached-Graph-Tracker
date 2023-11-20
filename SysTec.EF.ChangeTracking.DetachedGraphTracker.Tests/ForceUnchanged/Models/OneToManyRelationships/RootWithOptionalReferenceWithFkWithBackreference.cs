using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class RootWithOptionalReferenceWithFkWithBackreference : IdBase
{
    public int? OptionalItemId { get; set; }
    public OptionalReferenceItemWithBackreference OptionalItem { get; set; }
}