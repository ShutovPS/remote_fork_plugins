using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetCategoryCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string page = "0";
            if (data.Length > 3 && !string.IsNullOrWhiteSpace(data[3])) {
                page = data[3];
            }

            string response =
                HTTPUtility.GetRequest(PluginSettings.Settings.TrackerServer + $"/browse/{page}/{data[2]}/0/0");

            var regex = new Regex(PluginSettings.Settings.Regexp.GetSearchCenter);
            if (regex.IsMatch(response)) {
                regex = new Regex(PluginSettings.Settings.Regexp.GetSearchDataTopic);
                var result = regex.Matches(response);
                if (result.Count > 0) {
                    foreach (Match match in result) {
                        string torrentUrl = string.Empty;
                        regex = new Regex(PluginSettings.Settings.Regexp.GetSearchTorrent);
                        if (regex.IsMatch(match.Value)) {
                            torrentUrl = regex.Match(match.Value).Groups[2].Value;
                        }
                        string magnetUrl = string.Empty;
                        regex = new Regex(PluginSettings.Settings.Regexp.GetSearchMagnet);
                        if (regex.IsMatch(match.Value)) {
                            magnetUrl = regex.Match(match.Value).Groups[2].Value;
                        }
                        regex = new Regex(PluginSettings.Settings.Regexp.GetSearchName);

                        var item = new Item {
                            Link =
                                $"pagefilm{PluginSettings.Settings.Separator}{torrentUrl}{PluginSettings.Settings.Separator}{magnetUrl}",
                            Name = regex.Match(match.Value).Groups[2].Value,
                            ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                            Description = GetSearchCommand.GetDescription(match.Value)
                        };
                        items.Add(item);
                    }
                }
            }

            regex = new Regex(string.Format(PluginSettings.Settings.Regexp.GetCategoryNextPage, data[2],
                int.Parse(page ?? "0") + 1));
            if (regex.IsMatch(response)) {
                Rutor.NextPageUrl =
                    $"category{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}{regex.Match(response).Groups[2].Value}";
            }

            return items;
        }
    }
}
