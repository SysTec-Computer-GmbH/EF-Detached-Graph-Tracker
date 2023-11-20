using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ImplementationShowcase.Models;

public class Entity : IdBase
{
    public string? Text { get; set; }
}