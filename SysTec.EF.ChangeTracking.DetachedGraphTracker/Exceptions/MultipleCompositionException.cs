using SysTec.EF.ChangeTracking.DetachedGraphTracker.Attributes;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

/// <summary>
/// Exception thrown when multiple compositions for a type with the same key are found in the graph.
/// </summary>
public class MultipleCompositionException : GraphHandlerException
{
    internal MultipleCompositionException(Type type)
        : base(
            $"Multiple compositions found for type {type.Name}. Only one composition per graph is allowed. Please use the {nameof(UpdateAssociationOnly)} to have multiple nodes of the same type.")
    {
    }
}