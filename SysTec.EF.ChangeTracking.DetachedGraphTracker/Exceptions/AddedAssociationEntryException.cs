using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

/// <summary>
/// Exception thrown when an entity is tracked in an added state (no key set) but is annotated with the <see cref="UpdateAssociationOnly"/> (with the <see cref="AddedAssociationEntryBehavior"/> set to <see cref="AddedAssociationEntryBehavior.Throw"/>).
/// </summary>
public class AddedAssociationEntryException : GraphHandlerException
{
    internal AddedAssociationEntryException(TrackedAssociationEntityEntry association) : base("The entity " +
        $"{association.EntityEntry.Entity.GetType().Name} with key(s) {association.EntityEntry.GetKeysDebugString()} " +
        $"and inbound navigation {association.InboundNavigationEntry.Metadata.Name} " +
        $"cannot be tracked in an added state because it is annotated with the {nameof(UpdateAssociationOnly)}. " +
        $"Make sure the entity is persisted in the database before using it in a navigation property annotated with the {nameof(UpdateAssociationOnly)}.")
    {
    }
}