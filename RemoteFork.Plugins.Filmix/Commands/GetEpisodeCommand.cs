using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetEpisodeCommand : ICommand {
        public const string KEY = "episode";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();
            GetEpisodes(items, WebUtility.UrlDecode(data[2]));
            return items;
        }

        public static void GetEpisodes(List<Item> items, string url) {
            var episodes = url.Split(",");

            var baseItem = new Item() {
                Type = ItemType.FILE,
                ImageLink = PluginSettings.Settings.Icons.IcoVideo
            };

            foreach (string episode in episodes) {
                var match = new Regex(PluginSettings.Settings.Regexp.FileQualityArray).Match(episode);

                if (PluginSettings.Settings.IgnoreQualities != null &&
                    !PluginSettings.Settings.IgnoreQualities.Contains(match.Groups[2].Value)) {
                    var item = new Item(baseItem) {
                        Name = match.Groups[2].Value,
                        Link = match.Groups[3].Value
                    };
                    items.Add(item);
                }
            }
        }
    }
}
