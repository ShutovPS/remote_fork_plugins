using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetCategoryCommand : ICommand {
        public const string KEY = "category";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            string type = data[2];

            if (data.Length < 5) {
                var item = new Item() {
                    Name = "Поиск",
                    Type = ItemType.DIRECTORY,
                    Link =
                        $"{SearchCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}{data[3]}",
                    SearchOn = "Поиск",
                    ImageLink = PluginSettings.Settings.Icons.IcoSearch
                };
                items.Add(item);
            }

            switch (type) {
                case "serials":
                    items.AddRange(GetSerialsItems(data));
                    break;
                case "full":
                    break;
            }

            return items;
        }

        private static IEnumerable<Item> GetSerialsItems(params string[] data) {
            var items = new List<Item>();

            string url = WebUtility.UrlDecode(data[3]);

            if (data.Length < 5) {
                var item = new Item() {
                    Name = "Каталог",
                    Link = $"{GetCatalogCommand.KEY}{PluginSettings.Settings.Separator}{url}",
                    Type = ItemType.DIRECTORY

                };
                items.Add(item);
            }

            string response = HTTPUtility.GetRequest(url);

            items.AddRange(GetSerialsItems(response));

            return items;
        }

        public static IEnumerable<Item> GetSerialsItems(string htmlText) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.LeftCol);
            if (regex.IsMatch(htmlText)) {
                string leftCol = regex.Match(htmlText).Groups[2].Value;

                regex = new Regex(PluginSettings.Settings.Regexp.Categories);

                foreach (Match match in regex.Matches(leftCol)) {
                    items.Add(GetItem(match.Value));
                }

                regex = new Regex(PluginSettings.Settings.Regexp.NavBar);
                if (regex.IsMatch(htmlText)) {
                    string navigation = regex.Match(htmlText).Value;
                    regex = new Regex(PluginSettings.Settings.Regexp.FilmUrl);
                    if (regex.IsMatch(navigation)) {
                        HDSerials.NextPageUrl =
                            $"{KEY}{PluginSettings.Settings.Separator}serials{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(regex.Match(navigation).Groups[2].Value)}{PluginSettings.Settings.Separator}";
                    }
                }
            }

            return items;
        }

        private static Item GetItem(string text) {
            string title = string.Empty;
            string link = string.Empty;
            string image = string.Empty;
            string series = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.TitleDescription);
            if (regex.IsMatch(text)) {
                title = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.LinkDescription);
            if (regex.IsMatch(text)) {
                link = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.ImageDescription);
            if (regex.IsMatch(text)) {
                image = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex(PluginSettings.Settings.Regexp.SeriesDescription);
            if (regex.IsMatch(text)) {
                series = regex.Match(text).Groups[2].Value;
            }

            string description =
                $"<img src=\"{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title}</span><br>{series}";

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                Name = $"{title} ({series})",
                Link =
                    $"{GetFilmCommand.KEY}{PluginSettings.Settings.Separator}translations{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(link)}",
                Description = description
            };

            return item;
        }
    }
}
