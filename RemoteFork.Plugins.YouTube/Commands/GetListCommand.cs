using System.Collections.Generic;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetListCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string pageToken = string.Empty;

            if (!string.IsNullOrWhiteSpace(data[3])) {
                pageToken = "&pageToken=" + data[3];
            }

            string response =
                HTTPUtility.GetRequest(
                    $"{PluginSettings.Settings.ApiServer}/search?part=snippet&order={data[2]}&maxResults=50{pageToken}&key={PluginSettings.Settings.ApiKey}");

            items.AddRange(GetSearchCommand.GetVideoList(response,
                $"list{PluginSettings.Settings.Separator}{data[2]}"));

            return items;
        }
    }
}
