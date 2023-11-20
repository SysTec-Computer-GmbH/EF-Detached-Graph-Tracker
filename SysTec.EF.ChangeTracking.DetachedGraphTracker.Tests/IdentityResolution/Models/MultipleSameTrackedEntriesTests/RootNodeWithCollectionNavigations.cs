using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleSameTrackedEntriesTests;

public class RootNodeWithCollectionNavigations : IdBase
{
    [ForceAggregation] public List<Entity> A_Aggregations { get; set; } = new();

    public List<Entity> B_Compositions { get; set; } = new();

    [ForceAggregation] public List<Entity> C_Aggregations { get; set; } = new();
}