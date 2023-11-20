using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Polymorphism.Models;

public abstract class BaseEntity : IdBase, ICloneable
{
    public abstract string Text { get; set; }

    public string Discriminator { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}