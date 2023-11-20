using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

public class RootNodeWithOptionalSimpleListAndForceDelete : IdBase, ICloneable
{
    public string Text { get; set; }

    [ForceDeleteOnMissingEntries] public List<OptionalListItem> ListItems { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithOptionalSimpleListAndForceDelete)MemberwiseClone();
        clone.ListItems = ListItems.Select(x => (OptionalListItem)x.Clone()).ToList();
        return clone;
    }
}