using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.ForceAggregation.Models.Subtree;

public class AggregationRoot : IdBase, ICloneable
{
    public string Text { get; set; }

    public Entity? Composition { get; set; }

    public object Clone()
    {
        var clone = (AggregationRoot)MemberwiseClone();
        clone.Composition = (Entity)Composition?.Clone();
        return clone;
    }
}