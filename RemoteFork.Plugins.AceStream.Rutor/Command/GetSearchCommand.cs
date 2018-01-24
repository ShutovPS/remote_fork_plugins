using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetSearchCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            string response =
                HTTPUtility.GetRequest(PluginSettings.Settings.TrackerServer + $"/search/0/0/000/0/{data[2]}");

            var items = new List<Item>();

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
                            Description = GetDescription(match.Value)
                        };
                        items.Add(item);
                    }
                } else {
                    return NonSearch();
                }
            } else {
                return NonSearch();
            }

            return items;
        }

        public static string GetDescription(string html) {
            string title = null;
            string size = null;

            var regex = new Regex(PluginSettings.Settings.Regexp.GetSearchName);
            if (regex.IsMatch(html)) {
                title = regex.Match(html).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetSearchSize);
            if (regex.IsMatch(html)) {
                size = regex.Match(html).Groups[2].Value;
            }

            return "<span style=\"color:#3090F0\">" + title + "</span><br>Size: " + PluginSettings.Settings.TrackerServer + size;
        }

        private static List<Item> NonSearch(bool category = false) {
            var items = new List<Item>();
            var item = new Item {
                Link = string.Empty,
                ImageLink = PluginSettings.Settings.Icons.IcoNofile
            };
            if (category) {
                item.Name = "<span style=\"color#F68648\">" + " - Здесь ничего нет - " + "</span>";
                item.Description = "Нет информации для отображения";
            } else {
                item.Name = "<span style=\"color#F68648\">" + " - Ничего не найдено - " + "</span>";
                item.Description = "Поиск не дал результатов";
            }

            items.Add(item);

            return items;
        }
    }
}
