using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Behavior;

public class RootWithDefaultThrowBehavior : IdBase
{
    [UpdateAssociationOnly]
    public Item ItemWithDefaultThrowBehavior { get; set; }
}