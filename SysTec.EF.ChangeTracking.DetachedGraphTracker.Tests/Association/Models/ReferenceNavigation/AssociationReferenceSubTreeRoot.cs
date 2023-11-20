using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.ReferenceNavigation;

public class AssociationReferenceSubTreeRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public AssociationReferenceSubTreeItemL1? ItemL1 { get; set; }

    public object Clone()
    {
        var clone = (AssociationReferenceSubTreeRoot)MemberwiseClone();
        clone.ItemL1 = (AssociationReferenceSubTreeItemL1)ItemL1?.Clone();
        return clone;
    }
}