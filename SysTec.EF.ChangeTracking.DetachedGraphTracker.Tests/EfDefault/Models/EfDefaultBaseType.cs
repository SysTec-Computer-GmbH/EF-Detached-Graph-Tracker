using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public abstract class EfDefaultBaseType : IdBase
{
    public string Discriminator { get; set; }
}