namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Exceptions;

/// <summary>
/// Any exception thrown by the <see cref="DetachedGraphTracker"/> will be of this type.
/// </summary>
public class GraphHandlerException : Exception
{
    internal GraphHandlerException(string message) : base(message)
    {
    }
}