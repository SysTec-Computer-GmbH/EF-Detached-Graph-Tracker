using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultRootWithCollection : IdBase
{
    public string Text { get; set; }

    public List<EfDefaultItem> Items { get; set; } = new();
}