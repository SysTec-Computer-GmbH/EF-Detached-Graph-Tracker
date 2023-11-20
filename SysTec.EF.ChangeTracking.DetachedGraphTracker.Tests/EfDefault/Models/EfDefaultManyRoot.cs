using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultManyRoot : IdBase
{
    public List<EfDefaultItemWithBackreference> Items { get; set; } = new();
}