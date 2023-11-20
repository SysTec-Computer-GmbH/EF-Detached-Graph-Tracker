using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class RootNodeWithFirstTrackedAggregationReference : IdBase, ICloneable
{
    [UpdateAssociationOnly] public TrackedItem? A_Tracked_Item { get; set; }

    public TrackedItem B_Tracked_Item { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithFirstTrackedAggregationReference)MemberwiseClone();
        clone.A_Tracked_Item = (TrackedItem)A_Tracked_Item?.Clone();
        clone.B_Tracked_Item = (TrackedItem)B_Tracked_Item.Clone();
        return clone;
    }
}