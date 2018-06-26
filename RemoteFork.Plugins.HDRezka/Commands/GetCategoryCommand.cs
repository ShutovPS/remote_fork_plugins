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

            items.AddRange(GetFilmsItems(WebUtility.UrlDecode(data[2])));

            return items;
        }

        public static IEnumerable<Item> GetFilmsItems(string url) {
            var items = new List<Item>();

            string response = HTTPUtility.GetRequest(url);

            items.AddRange(GetFilmsItemsFromHtml(response));

            return items;
        }

        public static IEnumerable<Item> GetFilmsItemsFromHtml(string htmlText, bool search = false) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.Categories);

            foreach (Match match in regex.Matches(htmlText)) {
                items.Add(GetItem(match.Value));
            }

            regex = new Regex(PluginSettings.Settings.Regexp.FilmUrl);
            if (regex.IsMatch(htmlText)) {
                string navigation = regex.Match(htmlText).Groups[2].Value;
                if (!string.IsNullOrEmpty(navigation)) {
                    HDRezka.NextPageUrl =
                        $"{KEY}{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(navigation)}";
                }
            }

            return items;
        }

        private static Item GetItem(string text) {
            string title = string.Empty;
            string link = string.Empty;
            string image = string.Empty;
            string series = string.Empty;
            string category = string.Empty;
            string info = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.FullDescription);
            if (regex.IsMatch(text)) {
                var match = regex.Match(text);

                title = match.Groups[13].Value;
                link = match.Groups[2].Value;
                image = match.Groups[4].Value;
                category = match.Groups[6].Value;
                info = match.Groups[15].Value;

                series = match.Groups[10].Value;
                if (!string.IsNullOrEmpty(series)) {
                    title = $"{title} ({series})";
                }
            }

            string description =
                $"<img src=\"{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title}</span><br>{category}<br>{info}<br>{series}";

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                Name = $"{title}",
                Link =
                    $"{GetFilmCommand.KEY}{PluginSettings.Settings.Separator}translations{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(link)}",
                Description = description
            };

            return item;
        }
    }
}
