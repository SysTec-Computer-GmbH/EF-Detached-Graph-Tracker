using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeRootNodeWithFirstTrackedComposition : IdBase, ICloneable
{
    public string Text { get; set; }

    public AdvancedSubTreeNode1? A_CompositionNode { get; set; }

    [ForceAggregation] public AdvancedSubTreeNode1? B_AggregationNode { get; set; }

    [ForceAggregation] public List<AdvancedSubTreeNode1> B_AggregationNodes { get; set; } = new();

    public object Clone()
    {
        var clone = (AdvancedSubTreeRootNodeWithFirstTrackedComposition)MemberwiseClone();
        clone.A_CompositionNode = (AdvancedSubTreeNode1?)A_CompositionNode?.Clone();
        clone.B_AggregationNode = (AdvancedSubTreeNode1?)B_AggregationNode?.Clone();
        clone.B_AggregationNodes = B_AggregationNodes.Select(x => (AdvancedSubTreeNode1)x.Clone()).ToList();
        return clone;
    }
}