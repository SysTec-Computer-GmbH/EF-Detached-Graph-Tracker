using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.GraphTraversal;

public class TraversableNode : IdBase, ICloneable, ITraversable
{
    public bool GotTraversed { get; set; }
    
    public object Clone()
    {
        return MemberwiseClone();
    }
}