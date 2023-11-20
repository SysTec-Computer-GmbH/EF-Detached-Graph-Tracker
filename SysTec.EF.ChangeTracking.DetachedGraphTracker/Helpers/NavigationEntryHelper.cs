using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class NavigationEntryHelper
{
    internal static bool IsShadowProperty(this NavigationEntry navigationEntry)
    {
        return navigationEntry.Metadata.IsShadowProperty();
    }

    internal static void KeepUnchangedStateForForceUnchangedNavigations(EntityEntryGraphNode node)
    {
        foreach (var referenceNavigation in node.Entry.References.Where(n => n.HasForceUnchangedAttribute()).Where(n => n.CurrentValue == null))
        {
            referenceNavigation.IsModified = false;
        }
    }

    internal static void SeverRelationshipsForNullValuesInReferenceNavigations(EntityEntryGraphNode node)
    {
        // Entity Framework does not automatically sever relationships in a disconnected scenario when a reference navigation property is set to null.
        // Either the navigation must be set to null AFTER the entity is attached to the context or the navigation must be marked as modified manually.
        // Setting the whole entity as modified is not sufficient.
        var nullValueReferenceNavigations = node.Entry.References
            .Where(r => r.TargetEntry == null)
            .Where(r => !r.HasForceUnchangedAttribute()); 

        foreach (var referenceNavigation in nullValueReferenceNavigations)
        {
            var hasNonRequiredRelationshipInReferenceEntry = node.Entry.Metadata.GetForeignKeys()
                .Where(k => k.GetNavigation(true) == referenceNavigation.Metadata)
                .Any(k => !k.IsRequired);

            if (hasNonRequiredRelationshipInReferenceEntry) referenceNavigation.IsModified = true;
        }
    }
}