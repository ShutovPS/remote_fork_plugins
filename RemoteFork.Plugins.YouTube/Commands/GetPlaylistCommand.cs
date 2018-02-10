using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Plugins.YouTubeAPI;

namespace RemoteFork.Plugins.Commands {
    public class GetPlaylistCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            string pageToken = string.Empty;

            if (!string.IsNullOrWhiteSpace(data[3])) {
                pageToken = "&pageToken=" + data[3];
            }

            string response =
                HTTPUtility.GetRequest(
                    $"{PluginSettings.Settings.ApiServer}/playlistItems?part=snippet&playlistId={data[2]}&maxResults=50{pageToken}&key={YouTube.API_KEY}");

            return GetVideoList(response, $"playlist{PluginSettings.Settings.Separator}{data[2]}");
        }

        public static List<Item> GetVideoList(string response, string url) {
            var searchListResponse = JsonConvert.DeserializeObject<PlaylistSnippet>(response);

            var items = new List<Item>();
            if (searchListResponse.items.Count > 0) {
                var ytItems = searchListResponse.items.OrderBy(i => i.snippet.position);

                foreach (var searchResult in ytItems) {
                    if (searchResult.snippet.thumbnails != null) {
                        var item = new Item() {
                            Name = searchResult.snippet.title,
                            Description = "<img src=\"" + searchResult.snippet.thumbnails.Last().Value.url + "\"/><br>" +
                                          searchResult.snippet.description + "<br>" + "Канал: " +
                                          searchResult.snippet.channelTitle
                        };
                        switch (searchResult.snippet.resourceId.kind) {
                            case "youtube#video":
                                items.Add(new Item(item) {
                                    ImageLink = PluginSettings.Settings.Icons.IcoVideo,
                                    Link =
                                        $"video{PluginSettings.Settings.Separator}{searchResult.snippet.resourceId.videoId}"
                                });
                                break;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(searchListResponse.nextPageToken)) {
                    YouTube.NextPageUrl = url + PluginSettings.Settings.Separator + searchListResponse.nextPageToken;
                }
            }

            return items;
        }
    }
}
