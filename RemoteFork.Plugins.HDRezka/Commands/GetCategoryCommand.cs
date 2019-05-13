using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using RemoteFork.Network;
using RemoteFork.Plugins.Settings;

namespace RemoteFork.Plugins {
    public class GetCategoryCommand : ICommand {
        public const string KEY = "category";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            GetFilmsItems(items, WebUtility.UrlDecode(data[2]));

            return items;
        }

        public static void GetFilmsItems(List<Item> items, string url) {
            string response = HTTPUtility.GetRequest(url);

            GetFilmsItemsFromHtml(items, response);
        }

        public static void GetFilmsItemsFromHtml(List<Item> items, string htmlText, bool search = false) {
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
            }

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(image)) {
                sb.AppendLine(
                    $"<div id=\"poster\" style=\"float: left; padding: 4px; background-color: #eeeeee; margin: 0px 13px 1px 0px;\"><img style=\"width: 180px; float: left;\" src=\"{image}\" /></div>");
            }

            sb.AppendLine($"<span style=\"color: #3366ff;\"><strong>{title}</strong></span><br>");

            if (!string.IsNullOrEmpty(info) && info.Length > 3) {
                sb.AppendLine(
                    $"<span style=\"color: #999999;\">{info}</span><br>");
            }

            if (!string.IsNullOrEmpty(series) && series.Length != 0) {
                sb.AppendLine($"<strong><span style=\"color: #ff9900;\">Серии:</span></strong> {series}<br />");
            }

            if (!string.IsNullOrEmpty(category) && category.Length > 3) {
                sb.AppendLine($"<strong><span style=\"color: #ff9900;\">Категория:</span></strong> {category}<br />");
            }

            string description = sb.ToString();

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                Name = $"{title} ({info})",
                Link =
                    $"{GetFilmCommand.KEY}{PluginSettings.Settings.Separator}translations{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(link)}",
                Description = description
            };

            return item;
        }
    }
}
