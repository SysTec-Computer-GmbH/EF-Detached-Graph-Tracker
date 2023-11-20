using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleAssociation;

public class CompositionItem : IdBase, ICloneable
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public AssociationItem? AssociationItem { get; set; }

    [UpdateAssociationOnly] public List<AssociationItem> AssociationItems { get; set; } = new();


    public object Clone()
    {
        var clone = (CompositionItem)MemberwiseClone();
        clone.AssociationItem = (AssociationItem)AssociationItem?.Clone();
        return clone;
    }
}