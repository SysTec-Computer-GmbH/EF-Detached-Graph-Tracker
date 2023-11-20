using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToOneRelationship;

public class OptionalOneItemTwo : IdBase
{
    public int? OptionalItemId { get; set; }

    [ForceKeepExistingRelationship]
    public OptionalOneItemOne OptionalItem { get; set; }
}