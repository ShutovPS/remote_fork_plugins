using System.Collections.Generic;
using System.Collections.Specialized;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class SearchCommand : ICommand {
        public const string KEY = "search";

        public List<Item> GetItems(IPluginContext context, params string[] data) {
            var items = new List<Item>();

            string page = "0";
            if (data.Length > 2) {
                page = data[2];
            }

            string url = $"{PluginSettings.Settings.Links.Site}/engine/ajax/sphinx_search.php";
            var searchData = new NameValueCollection() {
                {"scf", "fx"},
                {"story", context.GetRequestParams()["search"]},
                {"search_start", page},
                {"do", "search"},
                {"subaction", "search"},
            };

            var header = new Dictionary<string, string>() {
                {"X-Requested-With", "XMLHttpRequest"}
            };

            string response = HTTPUtility.PostRequest(url, Tools.Tools.QueryParametersToString(searchData), header);

            items.AddRange(GetCategoryCommand.GetFilmsItemsFromHtml(response));

            return items;
        }
    }
}
