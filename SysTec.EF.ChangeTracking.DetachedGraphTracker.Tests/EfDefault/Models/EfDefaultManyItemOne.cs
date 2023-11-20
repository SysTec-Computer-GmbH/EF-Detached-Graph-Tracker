using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultManyItemOne : IdBase
{
    public List<EfDefaultManyItemTwo> Items { get; set; } = new();
}