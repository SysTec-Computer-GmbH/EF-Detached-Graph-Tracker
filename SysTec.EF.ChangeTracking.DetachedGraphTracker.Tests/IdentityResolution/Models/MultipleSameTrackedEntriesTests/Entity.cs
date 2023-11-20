using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models.MultipleSameTrackedEntriesTests;

public class Entity : IdBase, ICloneable
{
    public string Text { get; set; }
    public object Clone()
    {
        return MemberwiseClone();
    }
}