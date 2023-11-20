using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution.Models;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.IdentityResolution;

public static class DataHelper
{
    public const string ASSOCIATION_NAME = "Association";
    public const string COMPOSITION_NAME = "Composition";

    public static SubTreeRootNode GetSubTreeRootNode(string compositionOrAssociationNaming)
    {
        return new SubTreeRootNode
        {
            Text = $"SubTree {compositionOrAssociationNaming} Root",
            ReferenceItem = new SubTreeReferenceItem
            {
                Text = $"SubTree {compositionOrAssociationNaming} Item",
                SubTreeChildItems = new List<SubTreeChildItem>
                {
                    new()
                    {
                        Text = $"SubTree {compositionOrAssociationNaming} Item Child 1"
                    },
                    new()
                    {
                        Text = $"SubTree {compositionOrAssociationNaming} Item Child 2"
                    }
                }
            },
            SubTreeListItems = new List<SubTreeListItem>
            {
                new()
                {
                    Text = $"SubTree List {compositionOrAssociationNaming} Item 1"
                },
                new()
                {
                    Text = $"SubTree List {compositionOrAssociationNaming} Item 2"
                }
            }
        };
    }
}