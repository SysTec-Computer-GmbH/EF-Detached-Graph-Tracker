using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public static class DataHelper
{
    public const string AGGREGATION_NAME = "Aggregation";
    public const string COMPOSITION_NAME = "Composition";

    public static SubTreeRootNode GetSubTreeRootNode(string compositionOrAggregationNaming)
    {
        return new SubTreeRootNode
        {
            Text = $"SubTree {compositionOrAggregationNaming} Root",
            ReferenceItem = new SubTreeReferenceItem
            {
                Text = $"SubTree {compositionOrAggregationNaming} Item",
                SubTreeChildItems = new List<SubTreeChildItem>
                {
                    new()
                    {
                        Text = $"SubTree {compositionOrAggregationNaming} Item Child 1"
                    },
                    new()
                    {
                        Text = $"SubTree {compositionOrAggregationNaming} Item Child 2"
                    }
                }
            },
            SubTreeListItems = new List<SubTreeListItem>
            {
                new()
                {
                    Text = $"SubTree List {compositionOrAggregationNaming} Item 1"
                },
                new()
                {
                    Text = $"SubTree List {compositionOrAggregationNaming} Item 2"
                }
            }
        };
    }
}