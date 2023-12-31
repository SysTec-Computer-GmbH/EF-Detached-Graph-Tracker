using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

public class RootNodeWithOptionalSimpleList : IdBase, ICloneable
{
    public string Text { get; set; }

    public virtual List<OptionalListItem> ListItems { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithOptionalSimpleList)MemberwiseClone();
        clone.ListItems = ListItems.Select(x => (OptionalListItem)x.Clone()).ToList();
        return clone;
    }
}