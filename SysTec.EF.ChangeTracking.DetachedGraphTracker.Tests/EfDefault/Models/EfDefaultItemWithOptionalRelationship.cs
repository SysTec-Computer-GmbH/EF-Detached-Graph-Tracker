using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultItemWithOptionalRelationship : IdBase
{
    public string Text { get; set; }

    public EfDefaultItem? OptionalRelationship { get; set; }
}