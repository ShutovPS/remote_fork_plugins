using System.Collections.Generic;
using System.Net;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    public class SearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            const string url = "http://sensfilm.xyz/index.php?do=search";

            string searchText = WebUtility.UrlEncode(context.GetRequestParams()["search"]);

            string searchData = "do=search&subaction=search&search_start=1&full_search=0&result_from=1&story=" +
                                searchText;

            string response = HTTPUtility.PostRequest(url, searchData);

            items.AddRange(GetCategoryCommand.GetFilmsItemsFromHtml(response));

            return items;
        }
    }
}
