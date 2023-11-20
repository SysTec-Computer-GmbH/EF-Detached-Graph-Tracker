using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.CollectionNavigation;

public class AssociationCollectionComplexRootNode : IdBase, ICloneable
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public AssociationCollectionSubTreeRoot SubTreeRoot { get; set; }

    public object Clone()
    {
        var clone = (AssociationCollectionComplexRootNode)MemberwiseClone();
        clone.SubTreeRoot = (AssociationCollectionSubTreeRoot)SubTreeRoot.Clone();
        return clone;
    }
}