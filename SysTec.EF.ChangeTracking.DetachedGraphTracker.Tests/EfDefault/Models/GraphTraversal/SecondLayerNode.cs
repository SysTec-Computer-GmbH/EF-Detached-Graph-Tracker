using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.GraphTraversal;

public class SecondLayerNode : IdBase, ITraversable
{
    public bool GotTraversed { get; set; }

    public TraversableNode A_Node { get; set; }
    
    public TraversableNode B_Node { get; set; }

    public List<TraversableNode> A_Nodes { get; set; } = new();

    public List<TraversableNode> B_Nodes { get; set; } = new();
}