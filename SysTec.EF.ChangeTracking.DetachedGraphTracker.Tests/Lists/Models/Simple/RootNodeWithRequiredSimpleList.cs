using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

public class RootNodeWithRequiredSimpleList : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<RequiredListItem> ListItems { get; set; }

    public object Clone()
    {
        var rootNode = (RootNodeWithRequiredSimpleList)MemberwiseClone();
        rootNode.ListItems = ListItems.Select(x => (RequiredListItem)x.Clone()).ToList();
        return rootNode;
    }
}