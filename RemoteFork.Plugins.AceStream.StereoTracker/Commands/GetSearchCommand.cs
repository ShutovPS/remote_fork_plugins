using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetSearchCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var responseData =
                HTTPUtility.PostBytesRequest(PluginSettings.Settings.TrackerServer + "/index.php?do=search",
                    $"do=search&subaction=search&search_start=1&full_search=1&result_from=1&story={data[2]}");

            string response = Encoding.GetEncoding(1251).GetString(responseData);

            var items = new List<Item>();
            var regex = new Regex(PluginSettings.Settings.Regexp.GetSearchCenter);
            var result = regex.Matches(response);

            if (result.Count > 0) {
                foreach (Match match in result) {
                    regex = new Regex(PluginSettings.Settings.Regexp.GetSearchDataTopic);
                    if (regex.IsMatch(match.Value)) {
                        string url = regex.Match(match.Value).Groups[2].Value;
                        var item = new Item {
                            Link =
                                $"pagefilm{PluginSettings.Settings.Separator}{url}",
                            Name = regex.Match(match.Value).Groups[4].Value,
                            ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                            Description = GetDescription(match.Value)
                        };
                        items.Add(item);
                    }
                }
            } else {
                return NonSearch();
            }

            return items;
        }

        public static string GetDescription(string html) {
            string title = null;
            string kinopoisk = null;
            string image = null;

            var regex = new Regex(PluginSettings.Settings.Regexp.GetSearchName);
            if (regex.IsMatch(html)) {
                title = regex.Match(html).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetSearchKinopoisk);
            if (regex.IsMatch(html)) {
                kinopoisk = regex.Match(html).Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetSearchImage);
            if (regex.IsMatch(html)) {
                image = regex.Match(html).Groups[2].Value;
            }

            return "<span style=\"color:#3090F0\">" + title + "</span><br><img src=\"" + PluginSettings.Settings.TrackerServer + image + "\" /><br><br> " + kinopoisk;
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
