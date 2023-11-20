using Microsoft.EntityFrameworkCore;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Database;
using SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.Models.Simple;

namespace SysTec.EF.ChangeTracking.DetachedGraphTracker.Tests.Lists.SimpleGraph;

public static class DataHelper
{
    public static RootNodeWithRequiredSimpleList GetRootNodeWithRequiredSimpleList()
    {
        return new RootNodeWithRequiredSimpleList
        {
            Text = "RootNode",
            ListItems = new List<RequiredListItem>
            {
                new() { Text = "Item1" },
                new() { Text = "Item2" },
                new() { Text = "Item3" }
            }
        };
    }

    public static RootNodeWithOptionalSimpleList GetRootNodeWithOptionalSimpleList()
    {
        return new RootNodeWithOptionalSimpleList
        {
            Text = "RootNode",
            ListItems = GetOptionalListItems()
        };
    }

    public static RootNodeWithOptionalSimpleListAndForceDelete GetRootNodeWithOptionalSimpleListAndForceDelete()
    {
        return new RootNodeWithOptionalSimpleListAndForceDelete
        {
            Text = "RootNode",
            ListItems = GetOptionalListItems()
        };
    }

    public static RootNodeWithOptionalSimpleListAndAssociation
        GetRootNodeWithOptionalSimpleListAndAssociation()
    {
        return new RootNodeWithOptionalSimpleListAndAssociation
        {
            Text = "RootNode",
            ListItems = GetOptionalListItems()
        };
    }

    public static List<OptionalListItem> GetOptionalListItems()
    {
        return new List<OptionalListItem>
        {
            new() { Text = "Item1" },
            new() { Text = "Item2" },
            new() { Text = "Item3" }
        };
    }

    public static async Task<RootNodeWithRequiredSimpleList> GetRootNodeWithRequiredSimpleListFromDbWithIncludesAsync(
        DbContext dbContext)
    {
        return await dbContext.Set<RootNodeWithRequiredSimpleList>()
            .Include(x => x.ListItems.OrderBy(i => i.Text))
            .SingleAsync();
    }

    public static async Task<RootNodeWithOptionalSimpleList> GetRootNodeWithOptionalSimpleListFromDbWithIncludesAsync(
        DbContext dbContext)
    {
        return await dbContext.Set<RootNodeWithOptionalSimpleList>()
            .Include(x => x.ListItems.OrderBy(i => i.Text))
            .SingleAsync();
    }

    public static async Task<RootNodeWithOptionalSimpleListAndForceDelete>
        GetRootNodeWithOptionalSimpleListAndForceDeleteAttributeFromDbWithIncludesAsync(ListTestsDbContext dbContext)
    {
        return await dbContext.RootNodesWithOptionalSimpleListAndForceDelete
            .Include(x => x.ListItems.OrderBy(i => i.Text))
            .SingleAsync();
    }

    public static async Task<RootNodeWithOptionalSimpleListAndAssociation>
        GetRootNodeWithOptionalSimpleListAndAssociationAttributeFromDbWithIncludesAsync(
            ListTestsDbContext dbContext)
    {
        return await dbContext.RootNodesWithOptionalSimpleListAndAssociation
            .Include(x => x.ListItems.OrderBy(i => i.Text))
            .SingleAsync();
    }
}