using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultSelfReferencingListItem : IdBase
{
    public string Text { get; set; }

    public EfDefaultRootNode EfDefaultRootNode { get; set; }

    public EfDefaultSelfReferencingListItem? Parent { get; set; }

    public List<EfDefaultSelfReferencingListItem> Children { get; set; } = new();
}