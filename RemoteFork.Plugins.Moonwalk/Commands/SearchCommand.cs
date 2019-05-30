using System.Collections.Generic;
using System.Net;
using RemoteFork.Items;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class SearchCommand : ICommand {
        public const string KEY = "search";

        public void GetItems(PlayList playList, IPluginContext context, Dictionary<string, string> data) {
            string url = $"{PluginSettings.Settings.Links.Api}/videos.json";

            string searchText = WebUtility.UrlEncode(context.GetRequestParams()["search"]);

            string searchData = $"?api_token={PluginSettings.Settings.Api.Key}&title={searchText}";

            string response = HTTPUtility.GetRequest(url + searchData);

            GetCategoryCommand.GetFilmsItemsFromHtml(playList, url, response, true);
        }

        public static string CreateLink() {
            var data = new Dictionary<string, object>() {
                {Moonwalk.KEY, KEY}
            };

            return Moonwalk.CreateLink(data);
        }
    }
}
