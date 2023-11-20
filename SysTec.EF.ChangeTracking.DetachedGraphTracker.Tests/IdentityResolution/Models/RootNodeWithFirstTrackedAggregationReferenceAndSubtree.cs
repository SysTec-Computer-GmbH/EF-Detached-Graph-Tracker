using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class RootNodeWithFirstTrackedAssociationReferenceAndSubtree : IdBase, ICloneable
{
    [UpdateAssociationOnly] public SubTreeRootNode? A_Item { get; set; }

    public SubTreeRootNode B_Item { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithFirstTrackedAssociationReferenceAndSubtree)MemberwiseClone();
        clone.A_Item = (SubTreeRootNode)A_Item?.Clone();
        clone.B_Item = (SubTreeRootNode)B_Item.Clone();
        return clone;
    }
}