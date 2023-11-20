using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.WithIdentityResolution;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.WithIdentityResolution;

public static class DataHelper
{
    public static RootNodeWithMultipleCompositionsOfSameType GetRootNodeWithMultipleCompositionsOfSameType()
    {
        var rootNode = new RootNodeWithMultipleCompositionsOfSameType
        {
            CompositionItem = new Item { Text = "CompositionItem1" },
            CompositionItems = new List<Item>
            {
                new() { Text = "CompositionItem1" },
                new() { Text = "CompositionItem2" }
            }
        };
        return rootNode;
    }

    public static RootNodeWithMultipleNestedCompositionsOfSameType GetRootNodeWithMultipleNestedCompositionsOfSameType()
    {
        var rootNode = new RootNodeWithMultipleNestedCompositionsOfSameType
        {
            A_ExtraLayerItem = new ExtraLayerItem
            {
                CompositionItems = new List<Item>
                {
                    new() { Text = "CompositionItem1" },
                    new() { Text = "CompositionItem2" }
                }
            },
            B_CompositionItems = new List<Item>
            {
                new() { Text = "CompositionItem1" },
                new() { Text = "CompositionItem2" }
            }
        };
        return rootNode;
    }
}