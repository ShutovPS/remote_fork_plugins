using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetSearchCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            string responseFromServer =
                HTTPUtility.PostRequest(PluginSettings.Settings.TrackerServer + "/forum/search_cse.php", $"cx={Rutracker.CX}&1={data[2]}");

            var items = new List<Item>();
            var regex = new Regex(PluginSettings.Settings.Regexp.GetSearchCenter);
            var result = regex.Matches(responseFromServer);

            if (result.Count > 0) {
                foreach (Match match in result) {
                    regex = new Regex(PluginSettings.Settings.Regexp.GetSearchDataTopic);
                    string linkID = regex.Match(match.Value).Groups[0].Value;
                    regex = new Regex(linkID + PluginSettings.Settings.Regexp.GetSearchBBA);
                    var item = new Item {
                        Link =
                            $"pagefilm{PluginSettings.Settings.Separator}{PluginSettings.Settings.TrackerServer}/forum/viewtopic.php?t={linkID}",
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
            string title = null;
            string sizeFile = null;
            string seeders = null;
            string leechers = null;
            string category = null;
            string dataCreate = null;

            var regex = new Regex("(href=\"viewtopic.php.*?>)(.*?)(</a>)");
            if (regex.IsMatch(html)) {
                title = regex.Match(html).Groups[2].Value;
            }

            regex = new Regex("(<a class=\"small tr-dl dl-stub\")(.*)(&.*?;</a>)");
            if (regex.IsMatch(html)) {
                sizeFile = "<br>Размер: " + regex.Match(html).Groups[2].Value;
            }

            regex = new Regex("(title=\"Личи\"><b>)(.*?)(</b>)");
            if (regex.IsMatch(html)) {
                leechers = regex.Match(html).Groups[2].Value;
            }


            regex = new Regex("(<b class=\"seedmed\">)(.*?)(</b>)");
            if (regex.IsMatch(html)) {
                seeders = regex.Match(html).Groups[2].Value;
            }

            regex = new Regex("(<a class=\"gen f\")(.*?)(</a>)");
            if (regex.IsMatch(html)) {
                string subText = regex.Match(html).Value;
                var regexSub = new Regex("(?<=\">).*(.*)");
                if (regexSub.IsMatch(subText)) {
                    category = "<br><br>Раздел: " + regexSub.Match(subText).Value;
                }
            }

            regex = new Regex("(<td class=\"row4 small nowrap\")(.*?)(?=</td>)");
            if (regex.IsMatch(html)) {
                string subText = regex.Match(html).Value;
                var regexSub = new Regex("(?<=<p>).*(?=</p>)");
                if (regexSub.IsMatch(subText)) {
                    dataCreate = "<br><br>Создан: " + regexSub.Match(subText).Value;
                }
            }

            return "<span style=\"color:#3090F0\">" + title + "</span><br>" + sizeFile + "<br><br>Seeders: " + seeders +
                   "<br>Leechers: " + leechers + category + dataCreate;
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
