using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultItemWithUniqueConstraint : IdBase
{
    public bool UniqueConstraintWhenTrue { get; set; }
}