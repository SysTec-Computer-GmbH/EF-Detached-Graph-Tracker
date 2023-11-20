using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.Behavior;

public class RootWithDetachBehavior : IdBase
{
    [UpdateAssociationOnly(AddedAssociationEntryBehavior.Detach)]
    public Item ItemWithDetachBehavior { get; set; }
}