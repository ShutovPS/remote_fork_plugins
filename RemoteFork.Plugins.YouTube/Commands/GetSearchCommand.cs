using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;
using RemoteFork.Plugins.YouTubeAPI;

namespace RemoteFork.Plugins.Commands {
    public class GetSearchCommand : ICommand {
        public List<Item> GetItems(IPluginContext context, params string[] data) {
            if (string.IsNullOrWhiteSpace(data[2])) {
                data[2] = context.GetRequestParams()["search"];
            }

            string pageToken = string.Empty;

            if (!string.IsNullOrWhiteSpace(data[3])) {
                pageToken = "&pageToken=" + data[3];
            }

            string response =
                HTTPUtility.GetRequest(
                    $"{PluginSettings.Settings.ApiServer}/search?part=snippet&q={data[2]}&maxResults=50{pageToken}&key={YouTube.API_KEY}");

            return GetVideoList(response, $"search{PluginSettings.Settings.Separator}{data[2]}");
        }

        private static Item NonSearch() {
            return new Item {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.IcoChannel,
                Name = "<span style=\"color#F68648\">" + " - Ничего не найдено - " + "</span>",
                Description = "Поиск не дал результатов"
            };
        }

        public static List<Item> GetVideoList(string response, string url) {
            var searchListResponse = JsonConvert.DeserializeObject<SearchSnippet>(response);

            var items = new List<Item>();
            if (searchListResponse.items.Count > 0) {

                var videos = new List<Item>();
                var channels = new List<Item>();
                var playlists = new List<Item>();

                foreach (var searchResult in searchListResponse.items) {
                    if (searchResult.snippet.thumbnails != null) {
                        var item = new Item() {
                            Name = searchResult.snippet.title,
                            Description = "<img src=\"" + searchResult.snippet.thumbnails.Last().Value.url + "\"/><br>" +
                                          searchResult.snippet.description + "<br>" + "Канал: " +
                                          searchResult.snippet.channelTitle
                        };
                        switch (searchResult.id.kind) {
                            case "youtube#video":
                                videos.Add(new Item(item) {
                                    ImageLink = PluginSettings.Settings.Icons.IcoVideo,
                                    Link = $"video{PluginSettings.Settings.Separator}{searchResult.id.videoId}"
                                });
                                break;

                            case "youtube#channel":
                                channels.Add(new Item(item) {
                                    ImageLink = PluginSettings.Settings.Icons.IcoChannel,
                                    Link = $"channel{PluginSettings.Settings.Separator}{searchResult.id.channelId}"
                                });
                                break;

                            case "youtube#playlist":
                                playlists.Add(new Item(item) {
                                    ImageLink = PluginSettings.Settings.Icons.IcoPlaylist,
                                    Link = $"playlist{PluginSettings.Settings.Separator}{searchResult.id.playlistId}"
                                });
                                break;
                        }
                    }
                }

                items.AddRange(channels);
                items.AddRange(playlists);
                items.AddRange(videos);

                if (!string.IsNullOrWhiteSpace(searchListResponse.nextPageToken)) {
                    YouTube.NextPageUrl = url + PluginSettings.Settings.Separator + searchListResponse.nextPageToken;
                }
            } else {
                items.Add(NonSearch());
            }

            return items;
        }
    }
}
