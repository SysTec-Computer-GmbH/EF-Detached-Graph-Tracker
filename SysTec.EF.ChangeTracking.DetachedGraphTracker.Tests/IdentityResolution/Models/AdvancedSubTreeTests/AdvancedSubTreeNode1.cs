using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeNode1 : IdBase, ICloneable
{
    public string Text { get; set; }

    public AdvancedSubTreeNode2? CompositionNode { get; set; }

    public object Clone()
    {
        var clone = (AdvancedSubTreeNode1)MemberwiseClone();
        clone.CompositionNode = (AdvancedSubTreeNode2)CompositionNode?.Clone();
        return clone;
    }
}