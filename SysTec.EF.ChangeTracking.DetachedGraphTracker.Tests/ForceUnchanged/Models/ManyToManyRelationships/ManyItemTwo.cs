using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.ManyToManyRelationships;

public class ManyItemTwo : IdBase
{
    [ForceKeepExistingRelationship]
    public List<ManyItemOne> OptionalItems { get; set; } = new();
}