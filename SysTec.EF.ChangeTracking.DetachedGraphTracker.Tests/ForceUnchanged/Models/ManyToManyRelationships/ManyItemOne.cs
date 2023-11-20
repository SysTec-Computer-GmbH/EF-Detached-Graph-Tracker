using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.ManyToManyRelationships;

public class ManyItemOne : IdBase
{
    public List<ManyItemTwo> OptionalItems { get; set; } = new();
}