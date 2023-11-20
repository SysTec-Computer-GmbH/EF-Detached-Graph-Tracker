using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Inheritance;

public abstract class BaseTypeWithAbstractCollection : IdBase
{
    public abstract List<OptionalListItemWithBackreferenceToBaseType> Items { get; set; }
}