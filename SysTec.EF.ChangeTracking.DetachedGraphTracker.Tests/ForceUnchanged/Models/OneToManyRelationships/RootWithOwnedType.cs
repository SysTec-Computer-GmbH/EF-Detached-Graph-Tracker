using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceUnchanged.Models.OneToManyRelationships;

public class RootWithOwnedType : IdBase
{
    public OwnedType Owned { get; set; }
}