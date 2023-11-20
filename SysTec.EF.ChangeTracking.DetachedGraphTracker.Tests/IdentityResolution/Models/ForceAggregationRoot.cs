using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class ForceAggregationRoot : IdBase, ICloneable
{
    public ExtraLayerItem Composition { get; set; }

    public object Clone()
    {
        var clone = (ForceAggregationRoot)MemberwiseClone();
        clone.Composition = (ExtraLayerItem)Composition.Clone();
        return clone;
    }
}