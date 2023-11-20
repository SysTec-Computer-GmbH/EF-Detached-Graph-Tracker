using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Behavior;

public class RootWithDetachBehavior : IdBase
{
    [ForceAggregation(AddedForceAggregationBehavior.Detach)]
    public Item ItemWithDetachBehavior { get; set; }
}