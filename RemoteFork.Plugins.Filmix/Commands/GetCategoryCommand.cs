using System.Collections.Generic;
using System.Linq;
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

            GetFilmsItems(items, data[2], data[3], data[4]);

            return items;
        }

        public static void GetFilmsItems(List<Item> items, string type, string cat, string start = "0") {
            string url =
                $"{PluginSettings.Settings.Links.Site}/loader.php?do=cat&category={type}%2F{cat}&cstart={start}&requested_url={type}%2F{cat}%2Fpage%2F";

            var header = new Dictionary<string, string>() {
                {"X-Requested-With", "XMLHttpRequest"}
            };

            string response = HTTPUtility.GetRequest(url, header);

            url =
                $"{KEY}{PluginSettings.Settings.Separator}{type}{PluginSettings.Settings.Separator}{cat}{PluginSettings.Settings.Separator}{{0}}";

            GetFilmsItemsFromHtml(items, response, url);
        }

        public static void GetFilmsItemsFromHtml(List<Item> items, string htmlText, string nextUrl = null) {
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
        }

        private static Item GetItem(string text) {
            string title = string.Empty;
            string titleEn = string.Empty;
            string quality = string.Empty;
            string translation = string.Empty;
            string description = string.Empty;
            string link = string.Empty;
            string image = string.Empty;
            string[] categories = null;
            string info = string.Empty;
            string year = string.Empty;

            var regex = new Regex(PluginSettings.Settings.Regexp.FullDescription);
            if (regex.IsMatch(text)) {
                text = regex.Match(text).Value;

                regex = new Regex(PluginSettings.Settings.Regexp.Title);
                if (regex.IsMatch(text)) {
                    title = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.TitleOriginal);
                if (regex.IsMatch(text)) {
                    titleEn = regex.Match(text).Groups[2].Value;
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

                regex = new Regex(PluginSettings.Settings.Regexp.Categories);
                if (regex.IsMatch(text)) {
                    var category = regex.Match(text).Groups[2].Value;

                    regex = new Regex(PluginSettings.Settings.Regexp.Category);
                    if (regex.IsMatch(category)) {
                        categories = regex.Matches(category).Select(i => i.Groups[1].Value).ToArray();
                    }
                }

                regex = new Regex(PluginSettings.Settings.Regexp.AddInfo);
                if (regex.IsMatch(text)) {
                    info = regex.Match(text).Groups[2].Value;
                }

                regex = new Regex(PluginSettings.Settings.Regexp.Year);
                if (regex.IsMatch(text)) {
                    year = regex.Match(text).Groups[2].Value;
                }
            }

            if (!string.IsNullOrEmpty(titleEn)) {
                if (string.IsNullOrEmpty(title)) {
                    title = titleEn;
                } else {
                    title += $" / {titleEn}";
                }
            }
            if (!string.IsNullOrEmpty(year)) {
                title += $" ({year})";
            }

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(image)) {
                sb.AppendLine(
                    $"<div id=\"poster\" style=\"float: left; padding: 4px; background-color: #eeeeee; margin: 0px 13px 1px 0px;\"><img style=\"width: 180px; float: left;\" src=\"{image}\" /></div>");
            }

            sb.AppendLine($"<span style=\"color: #3366ff;\"><strong>{title}</strong></span><br />");

            if (!string.IsNullOrEmpty(info) && info.Length > 3) {
                sb.AppendLine(
                    $"<span style=\"color: #999999;\">{info}</span><br />");
            }

            if (!string.IsNullOrEmpty(quality) && quality.Length > 3) {
                sb.AppendLine($"<strong><span style=\"color: #ff9900;\">Качество:</span></strong> {quality}<br />");
            }

            if (!string.IsNullOrEmpty(translation) && translation.Length > 3) {
                sb.AppendLine($"<strong><span style=\"color: #ff9900;\">Перевод:</span></strong> {translation}<br />");
            }

            sb.AppendLine("<br />");

            if (categories != null && categories.Length > 0) {
                sb.AppendLine(
                    $"<span style=\"color: #339966;\"><strong>Жанры:</strong></span> {string.Join(", ", categories.Take(3))}<br />");
            }

            if (!string.IsNullOrEmpty(description)) {
                sb.AppendLine(
                    $"<p>{description}</p>");
            }

            string fulDescription = sb.ToString();

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                Name = $"{title}",
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Link =
                    $"{GetFilmCommand.KEY}{PluginSettings.Settings.Separator}translations{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(link)}",
                Description = fulDescription
            };

            return item;
        }
    }
}
