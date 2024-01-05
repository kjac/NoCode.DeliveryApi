using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Trees;
using Umbraco.Cms.Web.BackOffice.Trees;
using Umbraco.Cms.Web.Common.Attributes;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[Tree(Umbraco.Cms.Core.Constants.Applications.Settings, Constants.TreeAlias, TreeTitle = "No-Code Delivery API", TreeGroup = Umbraco.Cms.Core.Constants.Trees.Groups.Settings, SortOrder = 20)]
[PluginController(Constants.BackOfficeArea)]
public sealed class NoCodeDeliveryApiTreeController : TreeController
{
    private readonly IMenuItemCollectionFactory _menuItemCollectionFactory;

    public NoCodeDeliveryApiTreeController(
        ILocalizedTextService localizedTextService,
        UmbracoApiControllerTypeCollection umbracoApiControllerTypeCollection,
        IEventAggregator eventAggregator,
        IMenuItemCollectionFactory menuItemCollectionFactory)
        : base(localizedTextService, umbracoApiControllerTypeCollection, eventAggregator)
        => _menuItemCollectionFactory = menuItemCollectionFactory;

    protected override ActionResult<TreeNodeCollection> GetTreeNodes(string id, FormCollection queryStrings)
        => new TreeNodeCollection();

    protected override ActionResult<MenuItemCollection> GetMenuForNode(string id, FormCollection queryStrings)
        => _menuItemCollectionFactory.Create();

    protected override ActionResult<TreeNode?> CreateRootNode(FormCollection queryStrings)
    {
        ActionResult<TreeNode?> rootResult = base.CreateRootNode(queryStrings);
        if (rootResult.Result is not null)
        {
            return rootResult;
        }

        TreeNode? root = rootResult.Value;

        if (root is not null)
        {
            root.RoutePath = $"{Umbraco.Cms.Core.Constants.Applications.Settings}/{Constants.TreeAlias}/overview";
            root.Icon = Constants.TreeIcon;
            root.HasChildren = false;
            root.MenuUrl = null;
        }

        return root;
    }
}
