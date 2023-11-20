using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.CollectionNavigation;

public class ForceAggregationCollectionSubTreeItemL1 : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<ForceAggregationSubTreeItemL2> ItemsL2 { get; set; } = new();

    public object Clone()
    {
        var clone = (ForceAggregationCollectionSubTreeItemL1)MemberwiseClone();
        clone.ItemsL2 = ItemsL2.Select(i => (ForceAggregationSubTreeItemL2)i.Clone()).ToList();
        return clone;
    }
}