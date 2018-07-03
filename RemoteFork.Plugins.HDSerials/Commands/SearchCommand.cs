using System.Collections.Generic;
using System.Net;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    public class SearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string url = WebUtility.UrlDecode(data[3]);

            string searchText = WebUtility.UrlEncode(context.GetRequestParams()["search"]);

            string searchData = "do=search&subaction=search&search_start=1&full_search=0&result_from=1&story=" +
                                searchText;

            string response = HTTPUtility.PostRequest(string.Concat(url, "/index.php?do=search"), searchData);

            items.AddRange(GetCategoryCommand.GetSerialsItems(response));

            return items;
        }
    }
}
