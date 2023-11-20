using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models;

public class EfDefaultRootNode : IdBase
{
    public string Text { get; set; }

    public List<EfDefaultSelfReferencingListItem> Items { get; set; }
}