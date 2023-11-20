using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.CollectionNavigation;

public class ForceAggregationCollectionSubTreeRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<ForceAggregationCollectionSubTreeItemL1> ItemsL1 { get; set; } = new();

    public object Clone()
    {
        var clone = (ForceAggregationCollectionSubTreeRoot)MemberwiseClone();
        clone.ItemsL1 = ItemsL1.Select(i => (ForceAggregationCollectionSubTreeItemL1)i.Clone()).ToList();
        return clone;
    }
}