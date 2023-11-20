using System.ComponentModel.DataAnnotations.Schema;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Attributes.Models.ForceRelationshipUnchanged.Reference;

[Table("ForceRelationshipUnchangedReferenceItem")]
public class ReferenceItemWithBackreferenceToAbstractType : IdBase
{
    public string? Text { get; set; }
    
    public List<AbstractTypeWithAbstractReference> Items { get; set; } = new();
}