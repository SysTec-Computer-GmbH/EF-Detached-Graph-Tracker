using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.OneToMany;

public class ManyItem : IdBase
{
    public OneItem? OneItem { get; set; }
}