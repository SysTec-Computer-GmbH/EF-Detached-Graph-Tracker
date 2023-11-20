using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Enums;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Helpers;

internal static class AttributeHelper
{
    internal static bool HasForceDeleteAttribute(this NavigationEntry navigationEntry)
    {
        var navigationPropertyInfo = GetNullSafeConcreteNavigationEntryPropertyInfoOrThrow(navigationEntry);
        return navigationPropertyInfo.HasCustomAttribute(typeof(ForceDeleteOnMissingEntriesAttribute));
    }

    internal static bool InboundNavigationHasUpdateAssociationOnlyAttribute(this EntityEntryGraphNode entityEntryGraphNode)
    {
        var inboundNavigationPropertyInfo = GetNullSafeConcreteInboundNavigationPropertyInfoOrThrow(entityEntryGraphNode, nameof(InboundNavigationHasUpdateAssociationOnlyAttribute));
        return inboundNavigationPropertyInfo.HasCustomAttribute(typeof(UpdateAssociationOnly));
    }
    
    internal static AddedAssociationEntryBehavior InboundNavigationGetAddedAssociationEntryBehavior(this EntityEntryGraphNode entityEntryGraphNode)
    {
        var inboundNavigationPropertyInfo = GetNullSafeConcreteInboundNavigationPropertyInfoOrThrow(entityEntryGraphNode, nameof(InboundNavigationGetAddedAssociationEntryBehavior));
        
        if (!entityEntryGraphNode.InboundNavigationHasUpdateAssociationOnlyAttribute())
            // TODO: Export to throw helper
            ThrowHelper.ThrowInvalidOperationException(
                $"The navigation {inboundNavigationPropertyInfo.Name} does not have a {nameof(UpdateAssociationOnly)} but the method {nameof(InboundNavigationGetAddedAssociationEntryBehavior)} was called.");
        var updateAssociationOnlyAttribute = inboundNavigationPropertyInfo.GetCustomAttribute<UpdateAssociationOnly>();
        return updateAssociationOnlyAttribute!.AddedAssociationEntryBehavior;
    }
    
    internal static bool HasForceUnchangedAttribute(this NavigationEntry navigationEntry)
    {
        if (navigationEntry.IsShadowProperty()) return false;
        var navigationPropertyInfo = GetNullSafeConcreteNavigationEntryPropertyInfoOrThrow(navigationEntry);
        return navigationPropertyInfo.HasCustomAttribute(typeof(ForceKeepExistingRelationship));
    }

    private static bool HasCustomAttribute(this PropertyInfo propertyInfo, Type attributeType)
    {
        return propertyInfo.GetCustomAttributes(attributeType, false).Any();
    }

    private static PropertyInfo GetNullSafeConcreteInboundNavigationPropertyInfoOrThrow(EntityEntryGraphNode entityEntryGraphNode,
        string callerName)
    {
        ThrowHelper.ThrowIfInboundNavigationIsNull(entityEntryGraphNode, callerName);
        var inboundNavigation =  entityEntryGraphNode.InboundNavigation!;
        var sourceEntryType = entityEntryGraphNode.SourceEntry!.Entity.GetType();
        var inboundNavigationPropertyInfo = sourceEntryType.GetProperty(inboundNavigation.Name);
        ThrowHelper.ThrowIfPropertyInfoIsNull(inboundNavigationPropertyInfo);
        return inboundNavigationPropertyInfo!;
    }

    private static PropertyInfo GetNullSafeConcreteNavigationEntryPropertyInfoOrThrow(NavigationEntry navigationEntry)
    {
        var entityType = navigationEntry.EntityEntry.Entity.GetType();
        var navigationPropertyInfo = entityType.GetProperty(navigationEntry.Metadata.Name);
        ThrowHelper.ThrowIfPropertyInfoIsNull(navigationPropertyInfo);
        return navigationPropertyInfo!;
    }
}