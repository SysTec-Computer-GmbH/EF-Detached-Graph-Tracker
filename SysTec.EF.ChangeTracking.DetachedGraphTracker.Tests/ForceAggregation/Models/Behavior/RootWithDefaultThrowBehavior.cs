using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Behavior;

public class RootWithDefaultThrowBehavior : IdBase
{
    [UpdateAssociationOnly]
    public Item ItemWithDefaultThrowBehavior { get; set; }
}