using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class RootNodeWithExtraLayerAfterAggregation : IdBase, ICloneable
{
    public TrackedItem A_Composition { get; set; }

    [ForceAggregation] public ForceAggregationRoot? B_Aggregation { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithExtraLayerAfterAggregation)MemberwiseClone();
        clone.A_Composition = (TrackedItem)A_Composition.Clone();
        clone.B_Aggregation = (ForceAggregationRoot)B_Aggregation?.Clone();
        return clone;
    }
}