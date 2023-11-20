using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultItemWithBackreference : IdBase
{
    public int RootId { get; set; }
    public EfDefaultManyRoot Root { get; set; }
}