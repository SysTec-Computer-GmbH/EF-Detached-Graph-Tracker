using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.ManyToMany;

public class ManyItemTwo : IdBase
{
    public List<ManyItemOne> Items { get; set; } = new();
}