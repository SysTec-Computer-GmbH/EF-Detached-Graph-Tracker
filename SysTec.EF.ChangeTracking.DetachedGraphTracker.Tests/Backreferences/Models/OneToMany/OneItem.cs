using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.OneToMany;

public class OneItem : IdBase
{
    public List<ManyItem> ManyItems { get; set; } = new();
}