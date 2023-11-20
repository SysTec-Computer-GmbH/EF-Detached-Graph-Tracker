using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.Nested;

public abstract class AbstractBaseTypeWithBackreference : IdBase
{
    public int? BackreferenceId { get; set; }

    public ConcreteTypeWithAbstractTypeCollection Backreference { get; set; }

    public abstract object Clone();
}