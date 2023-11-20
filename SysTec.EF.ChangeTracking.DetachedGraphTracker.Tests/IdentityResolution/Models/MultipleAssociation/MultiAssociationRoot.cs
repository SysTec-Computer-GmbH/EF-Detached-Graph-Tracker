using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleAssociation;

public class MultiAssociationRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public CompositionItem CompositionItem { get; set; }

    [UpdateAssociationOnly] public AssociationItem? AssociationItem { get; set; }

    [UpdateAssociationOnly] public List<AssociationItem> AssociationItems { get; set; } = new();

    public object Clone()
    {
        var clone = (MultiAssociationRoot)MemberwiseClone();
        clone.CompositionItem = (CompositionItem)CompositionItem.Clone();
        clone.AssociationItem = (AssociationItem)AssociationItem?.Clone();
        clone.AssociationItems = AssociationItems.Select(x => (AssociationItem)x.Clone()).ToList();
        return clone;
    }
}