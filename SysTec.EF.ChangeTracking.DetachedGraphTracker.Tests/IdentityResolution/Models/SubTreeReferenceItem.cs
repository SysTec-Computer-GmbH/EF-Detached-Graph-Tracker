using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

public class SubTreeReferenceItem : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<SubTreeChildItem> SubTreeChildItems { get; set; }

    public object Clone()
    {
        var clone = (SubTreeReferenceItem)MemberwiseClone();
        clone.SubTreeChildItems = SubTreeChildItems.Select(x => (SubTreeChildItem)x.Clone()).ToList();
        return clone;
    }
}