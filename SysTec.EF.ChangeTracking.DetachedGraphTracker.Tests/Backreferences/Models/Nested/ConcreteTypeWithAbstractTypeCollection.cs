namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Backreferences.Models.Nested;

public class ConcreteTypeWithAbstractTypeCollection : AbstractBaseTypeWithBackreference
{
    public List<AbstractBaseTypeWithBackreference> Items { get; set; } = new();
    public override object Clone()
    {
        var clone = (ConcreteTypeWithAbstractTypeCollection)MemberwiseClone();
        clone.Items = Items.Select(x => (AbstractBaseTypeWithBackreference)x.Clone()).ToList();
        return clone;
    }
}