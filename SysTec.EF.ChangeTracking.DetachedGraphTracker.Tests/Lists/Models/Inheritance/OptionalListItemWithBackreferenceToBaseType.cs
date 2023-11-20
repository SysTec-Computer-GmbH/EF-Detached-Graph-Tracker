using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Inheritance;

public class OptionalListItemWithBackreferenceToBaseType : IdBase
{
    public int? BaseTypeId { get; set; }

    public BaseTypeWithAbstractCollection? BaseType { get; set; }
}