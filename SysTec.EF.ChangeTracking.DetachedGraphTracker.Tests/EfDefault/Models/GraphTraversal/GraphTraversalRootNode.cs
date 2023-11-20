using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.SharedModels;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.EfDefault.Models.GraphTraversal;

public class GraphTraversalRootNode : IdBase, ITraversable
{
    public bool GotTraversed { get; set; }

    public TraversableNode A_Node { get; set; }
    
    public TraversableNode B_Node { get; set; }

    public List<TraversableNode> A_Nodes { get; set; } = new();

    public List<TraversableNode> B_Nodes { get; set; } = new();

    public SecondLayerNode A_SecondLayerNode { get; set; }
    
    public SecondLayerNode B_SecondLayerNode { get; set; }
}