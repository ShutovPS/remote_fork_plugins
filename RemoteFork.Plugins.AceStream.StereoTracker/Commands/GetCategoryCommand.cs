using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetCategoryCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            var responseData = HTTPUtility.GetBytesRequest(PluginSettings.Settings.TrackerServer + data[2]);
            string response = Encoding.GetEncoding(1251).GetString(responseData);

            var regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTopics);
            string topics = regex.IsMatch(response) 
                ? regex.Match(response).Value 
                : response;
            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTopic);
            if (regex.IsMatch(topics)) {
                foreach (Match match in regex.Matches(topics)) {
                    regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTopicFilm);
                    if (regex.IsMatch(match.Value)) {
                        string url = regex.Match(match.Value).Groups[4].Value;
                        var item = new Item {
                            Link =
                                $"pagefilm{PluginSettings.Settings.Separator}{url}",
                            Name = regex.Match(match.Value).Groups[2].Value,
                            ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                            Description = GetSearchCommand.GetDescription(match.Value)
                        };
                        items.Add(item);
                    }
                }
            }

            regex = new Regex(string.Format(PluginSettings.Settings.Regexp.GetCategoryNextPage, PluginSettings.Settings.TrackerServer));
            if (regex.IsMatch(response)) {
                var matchGroups = regex.Match(response).Groups;
                StereoTracker.NextPageUrl =
                    $"category{PluginSettings.Settings.Separator}{matchGroups[2].Value}";
            }

            return items;
        }
    }
}
