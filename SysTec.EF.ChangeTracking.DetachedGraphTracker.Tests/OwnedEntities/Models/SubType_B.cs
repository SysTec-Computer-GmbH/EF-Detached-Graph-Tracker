namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.OwnedEntities.Models;

public class SubType_B : BaseType
{
    public override string Text { get; set; }

    public OwnedType OwnedType { get; set; }
}