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
                        $"{SearchSearialsCommand.KEY}{PluginSettings.Settings.Separator}{data[2]}{PluginSettings.Settings.Separator}{data[3]}",
                    SearchOn = "Поиск",
                    ImageLink =
                        "http://icons.iconarchive.com/icons/graphicloads/colorful-long-shadow/256/Search-icon.png"
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

            var regex = new Regex("(<div class=\"left-col\">)([\\s\\S]*?)(<div class=\"right-col\">)");
            if (regex.IsMatch(htmlText)) {
                string leftCol = regex.Match(htmlText).Groups[2].Value;

                regex = new Regex("(<div class=\"new-album-main\">)([\\s\\S]*?)(<\\/span><\\/a>)");

                foreach (Match match in regex.Matches(leftCol)) {
                    items.Add(GetItem(match.Value));
                }

                regex = new Regex("(<div class=\"navigation\">)([\\s\\S]*?)(<\\/div>)");
                if (regex.IsMatch(htmlText)) {
                    string navigation = regex.Match(htmlText).Value;
                    regex = new Regex("(<a href=\")(.*?)(\">[a-zA-Zа-яА-Я]*?<\\/a>\\s*<div)");
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

            var regex = new Regex("(<span class=\"[\\w-]*?-title\".*?>)(.*?)(<\\/span>)");
            if (regex.IsMatch(text)) {
                title = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex("(<a href=\")(.*?)(\"><span)");
            if (regex.IsMatch(text)) {
                link = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex("(<!--TBegin:)(.*?)(\\|)");
            if (regex.IsMatch(text)) {
                image = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex("(<div class=\"custom-update\">)(.*?)(<\\/div>)");
            if (regex.IsMatch(text)) {
                series = regex.Match(text).Groups[2].Value;
            }

            string description =
                $"<img src=\"{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title}</span><br>{series}";

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                Name = $"{title} ({series})",
                Link =
                    $"{GetSerialCommand.KEY}{PluginSettings.Settings.Separator}translations{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(link)}",
                Description = description
            };

            return item;
        }
    }
}
