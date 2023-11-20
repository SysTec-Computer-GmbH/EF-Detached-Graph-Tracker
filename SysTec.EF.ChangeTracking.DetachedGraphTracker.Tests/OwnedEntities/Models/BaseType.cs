using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Models;

public abstract class BaseType : IdBase
{
    public abstract string Text { get; set; }

    public string Discriminator { get; set; }
}