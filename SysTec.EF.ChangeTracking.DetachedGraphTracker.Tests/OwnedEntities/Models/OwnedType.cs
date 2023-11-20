using Microsoft.EntityFrameworkCore;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Models;

[Owned]
public class OwnedType : ICloneable
{
    public string Text { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}