using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Association.Models;

public class AssociationRootNode : IdBase
{
    public string Text { get; set; }

    [UpdateAssociationOnly] public AssociationType? AssociationTypeReference { get; set; }

    [UpdateAssociationOnly] public List<AssociationType> AssociationTypeCollection { get; set; } = new();
}