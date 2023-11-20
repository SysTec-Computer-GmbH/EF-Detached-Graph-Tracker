using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeRootNodeWithFirstTrackedComposition : IdBase, ICloneable
{
    public string Text { get; set; }

    public AdvancedSubTreeNode1? A_CompositionNode { get; set; }

    [UpdateAssociationOnly] public AdvancedSubTreeNode1? B_AssociationNode { get; set; }

    [UpdateAssociationOnly] public List<AdvancedSubTreeNode1> B_AssociationNodes { get; set; } = new();

    public object Clone()
    {
        var clone = (AdvancedSubTreeRootNodeWithFirstTrackedComposition)MemberwiseClone();
        clone.A_CompositionNode = (AdvancedSubTreeNode1?)A_CompositionNode?.Clone();
        clone.B_AssociationNode = (AdvancedSubTreeNode1?)B_AssociationNode?.Clone();
        clone.B_AssociationNodes = B_AssociationNodes.Select(x => (AdvancedSubTreeNode1)x.Clone()).ToList();
        return clone;
    }
}