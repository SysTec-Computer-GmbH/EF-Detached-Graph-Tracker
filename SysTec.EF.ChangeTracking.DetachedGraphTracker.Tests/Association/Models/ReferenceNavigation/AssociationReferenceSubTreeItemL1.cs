using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models.ReferenceNavigation;

public class AssociationReferenceSubTreeItemL1 : IdBase, ICloneable
{
    public string Text { get; set; }

    public AssociationSubTreeItemL2? ItemL2 { get; set; }

    public object Clone()
    {
        var clone = (AssociationReferenceSubTreeItemL1)MemberwiseClone();
        clone.ItemL2 = (AssociationSubTreeItemL2)ItemL2?.Clone();
        return clone;
    }
}