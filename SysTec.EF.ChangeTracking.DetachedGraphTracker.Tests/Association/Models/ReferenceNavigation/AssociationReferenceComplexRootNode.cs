using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.ReferenceNavigation;

public class AssociationReferenceComplexRootNode : IdBase, ICloneable
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public AssociationReferenceSubTreeRoot SubTreeRoot { get; set; }

    public object Clone()
    {
        var clone = (AssociationReferenceComplexRootNode)MemberwiseClone();
        clone.SubTreeRoot = (AssociationReferenceSubTreeRoot)SubTreeRoot.Clone();
        return clone;
    }
}