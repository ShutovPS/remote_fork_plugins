using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetSearchCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var header = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(PluginSettings.Settings.BbSession)) {
                header.Add("Cookie", "bb_session=" + PluginSettings.Settings.BbSession);
            }

            string responseFromServer =
                HTTPUtility.PostRequest(PluginSettings.Settings.TrackerServer + "/forum/tracker.php", $"nm={data[2]}",
                    header);

            var items = new List<Item>();
            var regex = new Regex(PluginSettings.Settings.Regexp.GetSearchCenter);
            var result = regex.Matches(responseFromServer);

            if (result.Count > 0) {
                foreach (Match match in result) {
                    regex = new Regex(PluginSettings.Settings.Regexp.GetSearchDataTopic);
                    string linkID = regex.Match(match.Value).Groups[2].Value;
                    regex = new Regex(string.Format(PluginSettings.Settings.Regexp.GetSearchLink, linkID));
                    var item = new Item {
                        Link =
                            $"pagefilm{PluginSettings.Settings.Separator}/forum/viewtopic.php?t={linkID}",
                        Name = regex.Match(match.Value).Groups[2].Value,
                        ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                        Description = GetDescription(match.Value)
                    };
                    items.Add(item);
                }
            } else {
                return NonSearch();
            }

            return items;
        }

        public static string GetDescription(string html) {
            string title = string.Empty;
            string sizeFile = string.Empty;
            string seeders = string.Empty;
            string leechers = string.Empty;
            string category = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.GetSearchTitle);
            if (regex.IsMatch(html)) {
                title = "<span style=\"color:#3090F0\">" + regex.Match(html).Groups[2].Value + "</span>";
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetSearchSize);
            if (regex.IsMatch(html)) {
                sizeFile = "<br>Размер: " + regex.Match(html).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetSearchLeechmed);
            if (regex.IsMatch(html)) {
                leechers = "Leechers: " + regex.Match(html).Groups[2].Value;
            }


            regex = new Regex(PluginSettings.Settings.Regexp.GetSearchSeedmed);
            if (regex.IsMatch(html)) {
                seeders = "Seeders: " + regex.Match(html).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetSearchCategory);
            if (regex.IsMatch(html)) {
                category = "Раздел: " + regex.Match(html).Groups[2].Value;
            }

            return title + "<br>" + sizeFile + "<br><br>" + seeders + "<br>" + leechers + "<br><br>" + category;
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
