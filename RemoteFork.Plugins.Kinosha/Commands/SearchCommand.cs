using System.Collections.Generic;
using System.Net;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class SearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string searchText = WebUtility.UrlEncode(context.GetRequestParams()["search"]);

            string response =
                HTTPUtility.GetRequest(
                    $"{PluginSettings.Settings.Links.Site}/search/f:{searchText}");

            items.AddRange(GetCategoryCommand.GetFilmsItemsFromHtml(response));

            return items;
        }
    }
}
