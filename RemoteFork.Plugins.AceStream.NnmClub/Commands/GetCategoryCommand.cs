using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins.Commands {
    public class GetCategoryCommand : ICommand {
        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string responseFromServer =
                HTTPUtility.GetRequest($"{PluginSettings.Settings.TrackerServer}/forum/{data[2]}");

            var regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryHead);

            foreach (Match match in regex.Matches(responseFromServer)) {
                regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTitle);
                var regexLink = new Regex(PluginSettings.Settings.Regexp.GetCategoryLink);
                var item = new Item {
                    Name = regex.Match(match.Value).Groups[2].Value,
                    ImageLink = PluginSettings.Settings.Icons.IcoTorrentFile,
                    Link =
                        $"pagefilm{PluginSettings.Settings.Separator}{regexLink.Match(match.Value).Groups[2].Value}",
                    Description = FormatDescription(match.Value)
                };

                items.Add(item);
            }

            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryNextPage);
            if (regex.IsMatch(responseFromServer)) {
                var matchGroups = regex.Match(responseFromServer).Groups;
                NnmClub.NextPageUrl =
                    $"category{PluginSettings.Settings.Separator}{matchGroups[3].Value + matchGroups[4].Value}";
            }

            NnmClub.IsIptv = false;
            return items;
        }

        private static string FormatDescription(string html) {
            string title = string.Empty;
            string infoFile = string.Empty;
            string infoFilms = string.Empty;
            string infoPro = string.Empty;
            string imagePath = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTitleA);
            if (regex.IsMatch(html)) {
                title = regex.Match(html).Groups[2].Value;
                title = $"</div><span style=\"color:#3090F0\">{title}</span>";
            }
            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryTitPims);
            if (regex.IsMatch(html)) {
                infoFile = $"Размер: {regex.Match(html).Groups[8].Value}";
            }
            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryVarA);
            if (regex.IsMatch(html)) {
                infoFilms = regex.Match(html).Groups[2].Value;
                infoFilms = $"<span style=\"color:#3090F0\">Описание: </span>{infoFilms}";
            }
            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryBrB);
            if (regex.IsMatch(html)) {
                infoPro = regex.Match(html).Groups[2].Value;
            }
            regex = new Regex(PluginSettings.Settings.Regexp.GetCategoryPortalImg);
            if (regex.IsMatch(html)) {
                imagePath = HttpUtility.UrlDecode(regex.Match(html).Groups[2].Value);
                imagePath = $"<img src=\"{imagePath}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/>";
            }

            return
                $"{imagePath}{title}<br>{infoFile}<br>{infoPro}<br>{infoFilms}";
        }
    }
}
