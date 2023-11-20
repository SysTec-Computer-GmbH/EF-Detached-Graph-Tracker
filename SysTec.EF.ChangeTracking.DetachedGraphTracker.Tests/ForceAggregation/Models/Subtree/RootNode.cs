using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Subtree;

public class RootNode : IdBase, ICloneable
{
    [UpdateAssociationOnly] public AggregationRoot? Aggregation { get; set; }

    [UpdateAssociationOnly] public List<AggregationRoot> Aggregations { get; set; } = new();

    public object Clone()
    {
        var clone = (RootNode)MemberwiseClone();
        clone.Aggregation = (AggregationRoot?)Aggregation?.Clone();
        clone.Aggregations = Aggregations.Select(x => (AggregationRoot)x.Clone()).ToList();
        return clone;
    }
}