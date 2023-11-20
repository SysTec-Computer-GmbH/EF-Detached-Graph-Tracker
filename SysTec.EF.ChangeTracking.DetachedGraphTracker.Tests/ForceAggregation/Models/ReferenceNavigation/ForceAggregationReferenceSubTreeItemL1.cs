using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.ReferenceNavigation;

public class ForceAggregationReferenceSubTreeItemL1 : IdBase, ICloneable
{
    public string Text { get; set; }

    public ForceAggregationSubTreeItemL2? ItemL2 { get; set; }

    public object Clone()
    {
        var clone = (ForceAggregationReferenceSubTreeItemL1)MemberwiseClone();
        clone.ItemL2 = (ForceAggregationSubTreeItemL2)ItemL2?.Clone();
        return clone;
    }
}