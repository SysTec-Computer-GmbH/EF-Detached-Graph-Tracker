using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

/// <summary>
/// Overrides the EF-behavior so that all EntityEntries inside the navigation property subtree will be marked as unchanged.
/// <br/>
/// No change is persisted in the database beside the change of relationship associations.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class UpdateAssociationOnly : Attribute
{
    internal AddedAssociationEntryBehavior AddedAssociationEntryBehavior { get; }
    
    /// <summary>
    /// Creates a new instance of <see cref="UpdateAssociationOnly"/>.
    /// </summary>
    /// <param name="addedAssociationEntryBehavior">
    /// The behavior of what happens if an entity has no key set (not existing in the database) but is added in a <see cref="UpdateAssociationOnly"/> navigation.
    /// <br/>
    /// Defaults to <see cref="AddedAssociationEntryBehavior.Throw"/>.
    /// </param>
    public UpdateAssociationOnly(AddedAssociationEntryBehavior addedAssociationEntryBehavior = AddedAssociationEntryBehavior.Throw)
    {
        AddedAssociationEntryBehavior = addedAssociationEntryBehavior;
    }
}