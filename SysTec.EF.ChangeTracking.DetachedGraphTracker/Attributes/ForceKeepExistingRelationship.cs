namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

/// <summary>
/// <para>
/// <list type="number">
/// <listheader><term><b>Does not change an existing reference relationship when:</b></term></listheader>
/// <item><term>The CurrentValue of the navigation property is null</term></item>
/// </list>
/// <b><i>IMPORTANT: If a value other than null is assigned to the navigation property, the relationship is changed.</i></b>
/// </para>
///
/// <para>
/// <list type="number">
/// <listheader><term><b> Does not change an existing relationship in a collection navigation when:</b></term></listheader>
/// <item><term>The collection is null or empty (no relationships are changed)</term></item>
/// <item><term> An item is missing in the collection (the relationship to the missing item is not changed) </term></item>
/// </list>
/// </para>
///
/// <b><i>IMPORTANT: New relationships can always be connected.</i></b>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ForceKeepExistingRelationship : Attribute
{
    
}