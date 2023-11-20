using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultItem : IdBase
{
    public string Text { get; set; }

    public List<EfDefaultItemWithOptionalRelationship> Backreferences { get; set; }
}