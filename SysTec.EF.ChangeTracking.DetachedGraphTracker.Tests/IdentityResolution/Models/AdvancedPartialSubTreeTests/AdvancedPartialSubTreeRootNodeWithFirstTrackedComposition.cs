using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedPartialSubTreeTests;

public class AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition : IdBase, ICloneable
{
    public AdvancedSubTreeNode2? A_Composition { get; set; }

    [ForceAggregation] public AdvancedSubTreeNode1? B_Aggregation { get; set; }

    public List<AdvancedSubTreeNode2> A_Compositions { get; set; } = new();

    [ForceAggregation] public List<AdvancedSubTreeNode1> B_Aggregations { get; set; } = new();

    public object Clone()
    {
        var clone = (AdvancedPartialSubTreeRootNodeWithFirstTrackedComposition)MemberwiseClone();
        clone.A_Composition = (AdvancedSubTreeNode2)A_Composition?.Clone();
        clone.B_Aggregation = (AdvancedSubTreeNode1)B_Aggregation?.Clone();
        clone.A_Compositions = A_Compositions.Select(x => (AdvancedSubTreeNode2)x.Clone()).ToList();
        clone.B_Aggregations = B_Aggregations.Select(x => (AdvancedSubTreeNode1)x.Clone()).ToList();
        return clone;
    }
}