using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class SubTreeRootNode : IdBase, ICloneable
{
    public string Text { get; set; }

    public SubTreeReferenceItem ReferenceItem { get; set; }

    public List<SubTreeListItem> SubTreeListItems { get; set; }

    public object Clone()
    {
        var clone = (SubTreeRootNode)MemberwiseClone();
        clone.ReferenceItem = (SubTreeReferenceItem)ReferenceItem.Clone();
        clone.SubTreeListItems = SubTreeListItems.Select(x => (SubTreeListItem)x.Clone()).ToList();
        return clone;
    }
}