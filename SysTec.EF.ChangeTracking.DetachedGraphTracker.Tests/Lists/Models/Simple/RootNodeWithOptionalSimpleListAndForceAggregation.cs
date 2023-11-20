using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

public class RootNodeWithOptionalSimpleListAndForceAggregation : IdBase, ICloneable
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public List<OptionalListItem> ListItems { get; set; }

    public object Clone()
    {
        var clone = (RootNodeWithOptionalSimpleListAndForceAggregation)MemberwiseClone();
        clone.ListItems = ListItems.Select(x => (OptionalListItem)x.Clone()).ToList();
        return clone;
    }
}