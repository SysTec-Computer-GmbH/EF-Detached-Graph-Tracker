using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.AdvancedSubTreeTests;

public class AdvancedSubTreeNode4 : IdBase, ICloneable
{
    public string Text { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}