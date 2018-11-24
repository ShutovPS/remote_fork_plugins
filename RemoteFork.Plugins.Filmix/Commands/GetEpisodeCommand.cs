using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetEpisodeCommand : ICommand {
        public const string KEY = "episode";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            return GetEpisodes(WebUtility.UrlDecode(data[2]));
        }

        public static List<Item> GetEpisodes(string url) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.FileQualityArray);
            if (regex.IsMatch(url)) {
                string qualities = regex.Match(url).Groups[2].Value;
                var regex2 = new Regex(PluginSettings.Settings.Regexp.FileQuality);
                var baseItem = new Item() {
                    Type = ItemType.FILE,
                    ImageLink = PluginSettings.Settings.Icons.IcoVideo
                };

                foreach (Match match in regex2.Matches(qualities)) {
                    if (PluginSettings.Settings.IgnoreQualities != null &&
                        !PluginSettings.Settings.IgnoreQualities.Contains(match.Value)) {
                        var item = new Item(baseItem) {
                            Name = match.Value,
                            Link = string.Concat(regex.Match(url).Groups[1].Value, match.Value,
                                regex.Match(url).Groups[3].Value)
                        };
                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}
