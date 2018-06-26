using System.Collections.Generic;
using System.Net;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class SearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string url = $"{PluginSettings.Settings.Links.Site}/index.php";

            string searchText = WebUtility.UrlEncode(context.GetRequestParams()["search"]);

            string searchData = "?do=search&subaction=search&q=" + WebUtility.UrlEncode(searchText);

            string response = HTTPUtility.GetRequest(url + searchData);

            items.AddRange(GetCategoryCommand.GetFilmsItemsFromHtml(response));

            return items;
        }
    }
}
