using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeListItem1 : IdBase, ICloneable
{
    public string Text { get; set; }

    public List<AdvancedSubTreeListItem2> CompositionListItems { get; set; }

    public object Clone()
    {
        var clone = (AdvancedSubTreeListItem1)MemberwiseClone();
        clone.CompositionListItems = CompositionListItems.Select(x => (AdvancedSubTreeListItem2)x.Clone()).ToList();
        return clone;
    }
}