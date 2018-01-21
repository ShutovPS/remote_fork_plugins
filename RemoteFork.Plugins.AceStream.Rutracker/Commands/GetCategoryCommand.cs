using System.Collections.Generic;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetCategoryCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string response = HTTPUtility.GetRequest(PluginSettings.Settings.TrackerServer + data[2]);

            var regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTable);
            string categories = string.Empty;
            if (regex.IsMatch(response)) {
                categories = regex.Match(response).Value;
            } else {
                regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryMinitable);
                if (regex.IsMatch(response)) {
                    categories = regex.Match(response).Value;
                }
            }
            regex = new Regex(PluginSettings.Settings.Regexp.GetCategorySubCategory);
            foreach (Match match in regex.Matches(categories)) {
                var item = new Item() {
                    Type = ItemType.DIRECTORY,
                    ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                    Name = match.Groups[4].Value,
                    Link = $"category{PluginSettings.Settings.Separator}/forum/{match.Groups[2].Value}",
                    Description = "<html><font face=\"Arial\" size=\"5\"><b>" + match.Groups[4].Value +
                                  "</font></b><p><img src=\"" + PluginSettings.Settings.Logo + "\" /> <p>"
                };
                items.Add(item);
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTopics);
            string topics = regex.IsMatch(response) 
                ? regex.Match(response).Value 
                : response;
            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTopic);
            if (regex.IsMatch(topics)) {
                foreach (Match match in regex.Matches(topics)) {
                    regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTopicFilm);
                    if (regex.IsMatch(match.Value)) {
                        var tempMatch = regex.Match(match.Value);
                        string id = tempMatch.Groups[4].Value;
                        string name = tempMatch.Groups[6].Value;
                        var item = new Item() {
                            Type = ItemType.DIRECTORY,
                            ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                            Name = name,
                            Link = $"pagefilm{PluginSettings.Settings.Separator}/forum/{id}",
                            Description = GetDescription(tempMatch.Value)
                        };
                        items.Add(item);
                    }
                }
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryNextPage);
            if (regex.IsMatch(response)) {
                var matchGroups = regex.Match(response).Groups;
                Rutracker.NextPageUrl =
                    $"category{PluginSettings.Settings.Separator}/forum/{matchGroups[3].Value + matchGroups[4].Value}";
            }

            return items;
        }

        private static string GetDescription(string text) {
            string sizeFile = null;
            string seeders = null;
            string leechers = null;

            var regex = new Regex(PluginSettings.Settings.Regexp.GetCategorySize);
            if (regex.IsMatch(text)) {
                sizeFile = "<br>Размер: " + regex.Match(text).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryLeechers);
            if (regex.IsMatch(text)) {
                leechers = regex.Match(text).Groups[2].Value;
            }


            regex = new Regex(PluginSettings.Settings.Regexp.GetCategorySeeders);
            if (regex.IsMatch(text)) {
                seeders = regex.Match(text).Groups[2].Value;
            }

            return "<span style=\"color:#3090F0\">" + "</span><br>" + sizeFile + "<br><br>Seeders: " + seeders +
                   "<br>Leechers: " + leechers;
        }
    }
}
