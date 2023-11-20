using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class RootNodeWithFirstTrackedCompositionCollection : IdBase, ICloneable
{
    public List<TrackedItem>? A_Tracked_Items { get; set; }

    [ForceAggregation] public TrackedItem? B_Tracked_Item { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithFirstTrackedCompositionCollection)MemberwiseClone();
        clone.A_Tracked_Items = A_Tracked_Items.Select(x => (TrackedItem)x.Clone()).ToList();
        clone.B_Tracked_Item = (TrackedItem)B_Tracked_Item?.Clone();
        return clone;
    }
}