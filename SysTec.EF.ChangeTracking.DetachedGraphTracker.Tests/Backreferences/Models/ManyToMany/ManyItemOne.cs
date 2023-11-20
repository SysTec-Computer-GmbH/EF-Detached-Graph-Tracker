using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.ManyToMany;

public class ManyItemOne : IdBase
{
    public List<ManyItemTwo> Items { get; set; } = new();
}