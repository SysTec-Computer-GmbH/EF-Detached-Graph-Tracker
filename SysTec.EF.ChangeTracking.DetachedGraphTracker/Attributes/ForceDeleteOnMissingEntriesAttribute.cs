namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

/// <summary>
/// <para>
/// Enforces that items that are missing in the current instance of a collection - but are still present in the database - are deleted from the database.
/// This is the EF default for required 1-to-many relationships.
/// <br/>
/// Works for collection navigation properties only.
/// </para>
/// <para>
/// Using this attribute can be useful if TPH is configured and a FK that should be required is nullable in the database.
/// </para>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ForceDeleteOnMissingEntriesAttribute : Attribute
{
}