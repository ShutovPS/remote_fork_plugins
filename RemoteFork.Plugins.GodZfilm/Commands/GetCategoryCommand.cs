using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using RemoteFork.Network;

namespace RemoteFork.Plugins {
    public class GetCategoryCommand : ICommand {
        public const string KEY = "category";

        public List<Item> GetItems(IPluginContext context = null, params string[] data) {
            var items = new List<Item>();

            items.AddRange(GetFilmsItems(data));

            return items;
        }

        private static IEnumerable<Item> GetFilmsItems(params string[] data) {
            var items = new List<Item>();

            string url = WebUtility.UrlDecode(data[2]);

            string response = HTTPUtility.GetRequest(url);

            items.AddRange(GetFilmsItems(response));

            return items;
        }

        public static IEnumerable<Item> GetFilmsItems(string htmlText, bool search = false) {
            var items = new List<Item>();

            var regex = new Regex("(<div class=\"main-news\">)([\\s\\S]*?)(<div class=\"main-news-more\">)");

            foreach (Match match in regex.Matches(htmlText)) {
                items.Add(GetItem(match.Value, search));
            }

            regex = new Regex("(<i class=\"next-nav1\"><a href=\")(.*?)(\">)");
            if (regex.IsMatch(htmlText)) {
                string navigation = regex.Match(htmlText).Groups[2].Value;
                if (!string.IsNullOrEmpty(navigation)) {
                    GodZfilm.NextPageUrl =
                        $"{KEY}{GodZfilm.SEPARATOR}{WebUtility.UrlEncode(navigation)}";
                }
            }

            return items;
        }

        private static Item GetItem(string text, bool search) {
            string title = string.Empty;
            string link = string.Empty;
            string image = string.Empty;
            string series = string.Empty;
            string translate = string.Empty;

            var regex = new Regex("(alt=\")(.*?)(\">)");
            if (regex.IsMatch(text)) {
                title = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex("(<a href=\")(.*?)(\"><img)");
            if (regex.IsMatch(text)) {
                link = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex("(<img src=\")(.*?)(\")");
            if (regex.IsMatch(text)) {
                image = regex.Match(text).Groups[2].Value;
            }

            regex = new Regex(search ? "(<center.*?\">)(.*?)(<.*\\/center>)" : "(<center.*?>)(.*?)(<.*\\/center>)");
            if (regex.IsMatch(text)) {
                series = regex.Match(text).Groups[2].Value;
                if (!string.IsNullOrEmpty(series)) {
                    title = $"{title} ({series})";
                }
            }

            regex = new Regex(search ? "(<div class=\"main-news-janr\".*?\">)(.*?)(<.*\\/div>)" : "(<h2><a href=\".*\">)(.*?)((<\\/font>)?<\\/a>)");
            if (regex.IsMatch(text)) {
                translate = regex.Match(text).Groups[2].Value;
                if (!string.IsNullOrEmpty(translate)) {
                    title = $"{title} ({translate})";
                }
            }

            string description =
                $"<img src=\"http://godzfilm.net{image}\" alt=\"\" align=\"left\" style=\"width:240px;float:left;\"/></div><span style=\"color:#3090F0\">{title}</span><br>{translate}<br>{series}";

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                Name = $"{title}",
                Link =
                    $"{GetFilmCommand.KEY}{GodZfilm.SEPARATOR}translations{GodZfilm.SEPARATOR}{WebUtility.UrlEncode(link)}",
                Description = description
            };

            return item;
        }
    }
}
