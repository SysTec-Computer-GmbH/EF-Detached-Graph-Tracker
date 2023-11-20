using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeRootNodeWithFirstTrackedAssociation : IdBase, ICloneable
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public AdvancedSubTreeNode1? A_AssociationNode { get; set; }

    [UpdateAssociationOnly] public List<AdvancedSubTreeNode1> A_AssociationNodes { get; set; } = new();

    public AdvancedSubTreeNode1? B_CompositionNode { get; set; }

    public object Clone()
    {
        var clone = (AdvancedSubTreeRootNodeWithFirstTrackedAssociation)MemberwiseClone();
        clone.A_AssociationNode = (AdvancedSubTreeNode1?)A_AssociationNode?.Clone();
        clone.B_CompositionNode = (AdvancedSubTreeNode1?)B_CompositionNode?.Clone();
        clone.A_AssociationNodes = A_AssociationNodes.Select(x => (AdvancedSubTreeNode1)x.Clone()).ToList();
        return clone;
    }
}