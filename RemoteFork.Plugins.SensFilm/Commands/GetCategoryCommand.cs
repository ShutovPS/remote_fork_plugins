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
                    SensFilm.NextPageUrl =
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
            string translate = string.Empty;

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
                if (!string.IsNullOrEmpty(series)) {
                    title = $"{title} ({series})";
                }
            }

            regex = new Regex(PluginSettings.Settings.Regexp.TranslateDescription);
            if (regex.IsMatch(text)) {
                translate = regex.Match(text).Groups[2].Value;
                if (!string.IsNullOrEmpty(translate)) {
                    title = $"{title} ({translate})";
                }
            }

            string description =
                $"<img src=\"http://sensfilm.xyz{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title}</span><br>{translate}<br>{series}";

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
