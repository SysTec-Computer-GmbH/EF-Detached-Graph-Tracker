using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class RootWithOptionalCollectionWithFkWithBackreference : IdBase
{
    public List<OptionalCollectionItemWithBackreference> OptionalItems { get; set; } = new();
}