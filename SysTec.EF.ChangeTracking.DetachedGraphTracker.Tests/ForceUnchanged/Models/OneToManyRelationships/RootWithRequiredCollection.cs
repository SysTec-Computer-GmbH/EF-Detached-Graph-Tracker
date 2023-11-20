using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class RootWithRequiredCollection : IdBase
{
    [ForceKeepExistingRelationship]
    public List<RequiredCollectionItem> RequiredItems { get; set; } = new();
}