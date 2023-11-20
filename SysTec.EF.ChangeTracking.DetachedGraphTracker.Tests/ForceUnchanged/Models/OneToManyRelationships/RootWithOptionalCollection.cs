using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class RootWithOptionalCollection : IdBase
{
    [ForceKeepExistingRelationship]
    public List<OptionalCollectionItem> OptionalItems { get; set; } = new();
}