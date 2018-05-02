using System.Collections.Generic;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetChannelCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            string pageToken = string.Empty;

            if (!string.IsNullOrWhiteSpace(data[3])) {
                pageToken = "&pageToken=" + data[3];
            }

            string response =
                HTTPUtility.GetRequest(
                    $"{PluginSettings.Settings.ApiServer}/search?part=snippet&channelId={data[2]}&order=date&maxResults=50{pageToken}&key={YouTube.API_KEY}");
            return GetSearchCommand.GetVideoList(response, $"channel{PluginSettings.Settings.Separator}{data[2]}");
        }
    }
}
