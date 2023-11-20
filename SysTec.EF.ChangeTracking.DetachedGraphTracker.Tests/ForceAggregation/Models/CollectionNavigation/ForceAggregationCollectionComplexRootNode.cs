using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.CollectionNavigation;

public class ForceAggregationCollectionComplexRootNode : IdBase, ICloneable
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public ForceAggregationCollectionSubTreeRoot SubTreeRoot { get; set; }

    public object Clone()
    {
        var clone = (ForceAggregationCollectionComplexRootNode)MemberwiseClone();
        clone.SubTreeRoot = (ForceAggregationCollectionSubTreeRoot)SubTreeRoot.Clone();
        return clone;
    }
}