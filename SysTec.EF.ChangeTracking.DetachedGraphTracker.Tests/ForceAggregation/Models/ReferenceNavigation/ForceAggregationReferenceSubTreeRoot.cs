using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.ReferenceNavigation;

public class ForceAggregationReferenceSubTreeRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public ForceAggregationReferenceSubTreeItemL1? ItemL1 { get; set; }

    public object Clone()
    {
        var clone = (ForceAggregationReferenceSubTreeRoot)MemberwiseClone();
        clone.ItemL1 = (ForceAggregationReferenceSubTreeItemL1)ItemL1?.Clone();
        return clone;
    }
}