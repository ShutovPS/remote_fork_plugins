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

            items.AddRange(GetFilmsItems(data[2], data[3], data[4]));

            return items;
        }

        public static IEnumerable<Item> GetFilmsItems(string type, string cat, string start = "0") {
            var items = new List<Item>();

            string url =
                $"{PluginSettings.Settings.Links.Site}/loader.php?do=cat&category={type}%2F{cat}&cstart={start}&requested_url={type}%2F{cat}%2Fpage%2F";
            
            var header = new Dictionary<string, string>() {
                {"X-Requested-With", "XMLHttpRequest"}
            };

            string response = HTTPUtility.GetRequest(url, header);

            url =
                $"{KEY}{PluginSettings.Settings.Separator}{type}{PluginSettings.Settings.Separator}{cat}{PluginSettings.Settings.Separator}{{0}}";

            items.AddRange(GetFilmsItemsFromHtml(response, url));

            return items;
        }

        public static IEnumerable<Item> GetFilmsItemsFromHtml(string htmlText, string nextUrl = null) {
            var items = new List<Item>();

            var regex = new Regex(PluginSettings.Settings.Regexp.FullDescription);

            foreach (Match match in regex.Matches(htmlText)) {
                items.Add(GetItem(match.Value));
            }

            if (!string.IsNullOrEmpty(nextUrl)) {
                regex = new Regex(PluginSettings.Settings.Regexp.NextPage);
                if (regex.IsMatch(htmlText)) {
                    string navigation = regex.Match(htmlText).Groups[2].Value;
                    if (!string.IsNullOrEmpty(navigation)) {
                        Filmix.NextPageUrl = string.Format(nextUrl, navigation);
                    }
                }
            }

            return items;
        }

        private static Item GetItem(string text) {
            string title = string.Empty;
            string quality = string.Empty;
            string translation = string.Empty;
            string description = string.Empty;
            string link = string.Empty;
            string image = string.Empty;
            string category = string.Empty;
            string info = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.FullDescription);
            if (regex.IsMatch(text)) {
                text = regex.Match(text).Value;

                regex = new Regex(PluginSettings.Settings.Regexp.Title);
                if (regex.IsMatch(text)) {
                    title = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.Quality);
                if (regex.IsMatch(text)) {
                    quality = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.Translation);
                if (regex.IsMatch(text)) {
                    translation = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.Description);
                if (regex.IsMatch(text)) {
                    description = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.Link);
                if (regex.IsMatch(text)) {
                    link = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.Poster);
                if (regex.IsMatch(text)) {
                    image = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.Category);
                if (regex.IsMatch(text)) {
                    category = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.AddInfo);
                if (regex.IsMatch(text)) {
                    info = regex.Match(text).Groups[2].Value;
                }
            }

            string fulDescription =
                $"<img src=\"{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title} ({info})</span><br>{quality}<br>{translation}<br>{description}<br>{category}";

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                Name = $"{title}",
                Link =
                    $"{GetFilmCommand.KEY}{PluginSettings.Settings.Separator}translations{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(link)}",
                Description = fulDescription
            };

            return item;
        }
    }
}
