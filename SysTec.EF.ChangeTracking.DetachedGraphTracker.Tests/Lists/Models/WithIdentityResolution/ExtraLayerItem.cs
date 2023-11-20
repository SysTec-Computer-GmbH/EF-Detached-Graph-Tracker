using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

public class ExtraLayerItem : IdBase, ICloneable
{
    public List<Item> CompositionItems { get; set; }

    public object Clone()
    {
        var clone = (ExtraLayerItem)MemberwiseClone();
        clone.CompositionItems = CompositionItems.Select(x => (Item)x.Clone()).ToList();
        return clone;
    }
}