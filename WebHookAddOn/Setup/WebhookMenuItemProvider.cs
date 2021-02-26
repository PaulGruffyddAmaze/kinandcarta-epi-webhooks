using EPiServer;
using EPiServer.Security;
using EPiServer.Shell;
using EPiServer.Shell.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinAndCarta.Connect.Webhooks.Setup
{
    [MenuProvider]
    public class WebhookMenuItemProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var menuItems = new List<MenuItem>();
            menuItems.Add(new UrlMenuItem("Webhooks",
                MenuPaths.Global + "/cms/cmsMenuItem",
                Paths.ToResource("KinAndCarta.Connect.Webhooks", ""))
            {
                SortIndex = SortIndex.First + 25,
                IsAvailable = (request) => PrincipalInfo.HasAdminAccess
            });

            menuItems.Add(new UrlMenuItem("Webhooks",
                MenuPaths.Global + "/cms/cmsMenuItem",
                Paths.ToResource("KinAndCarta.Connect.Webhooks", "WebhookAdmin/edit"))
            {
                SortIndex = int.MaxValue,
                IsAvailable = (request) => false
            });

            return menuItems;
        }
    }
}