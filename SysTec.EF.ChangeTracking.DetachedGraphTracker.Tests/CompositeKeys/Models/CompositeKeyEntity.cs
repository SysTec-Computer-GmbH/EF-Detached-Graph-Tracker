using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.CompositeKeys.Models;

[PrimaryKey(nameof(Id), nameof(Key2))]
public class CompositeKeyEntity : ICloneable
{
    public int Id { get; set; }

    public int Key2 { get; set; }

    public string Text { get; set; }

    public CompositeForeignKeyCompositionEntity? A_Composition_Item { get; set; }

    public List<CompositeForeignKeyCompositionEntity> B_Composition_Items { get; set; } = new();

    [UpdateAssociationOnly] public CompositeForeignKeyAggregationEntity? A_Aggregation_Item { get; set; }

    [UpdateAssociationOnly] public List<CompositeForeignKeyAggregationEntity> B_Aggregation_Items { get; set; }

    public object Clone()
    {
        var clone = (CompositeKeyEntity)MemberwiseClone();
        clone.A_Aggregation_Item = (CompositeForeignKeyAggregationEntity)A_Aggregation_Item?.Clone();
        clone.B_Aggregation_Items =
            B_Aggregation_Items.Select(x => (CompositeForeignKeyAggregationEntity)x.Clone()).ToList();
        clone.A_Composition_Item = (CompositeForeignKeyCompositionEntity)A_Composition_Item?.Clone();
        clone.B_Composition_Items =
            B_Composition_Items.Select(x => (CompositeForeignKeyCompositionEntity)x.Clone()).ToList();
        return clone;
    }
}