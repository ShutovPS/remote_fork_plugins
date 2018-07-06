using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
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

            items.AddRange(GetFilmsItemsFromHtml(url, response));

            return items;
        }

        public static IEnumerable<Item> GetFilmsItemsFromHtml(string url, string response, bool search = false) {
            var items = new List<Item>();

            response = Regex.Match(response, "\\[.*\\]").Value;

            var films = JsonConvert.DeserializeObject<FilmModel[]>(response);
            foreach (var film in films) {
                var item = GetItem(film);
                if (items.Any(i => i.Link == item.Link)) {
                    continue;
                }
                items.Add(item);
            }

            if (!search) {
                var regex = new Regex("(\\&page\\=)(\\d+)");
                if (regex.IsMatch(url)) {
                    int page = int.Parse(regex.Match(url).Groups[2].Value);
                    url = regex.Replace(url, $"&page={page + 1}");
                } else {
                    url += "&page=2";
                }

                Moonwalk.NextPageUrl = $"{KEY}{PluginSettings.Settings.Separator}{url}";
            }

            return items;
        }

        private static Item GetItem(FilmModel film) {
            if (film.Serial != null) {
                film = film.Serial;
            }
            string title = film.TitleRu;
            if (!string.IsNullOrEmpty(film.TitleEn)) {
                title += $" ({film.TitleEn})";
            }

            string translate = film.Translator;
            string quality = film.SourceType;
            if (string.IsNullOrEmpty(quality)) {
                quality = string.Empty;
            }
            int year = film.Year;
            string seasons = string.Empty;

            string link = film.IframeUrl;
            string token = film.Token;

            if (film.SeasonEpisodesCount != null) {
                seasons = $"Сезон: {film.SeasonEpisodesCount.Length}";
            }

            string category = film.Category;
            string info = film.MaterialData?.Description;

            string description =
                $"<span style=\"color:#3090F0\">{title}</span><br>{year}<br>{category}<br>{quality} ({translate})<br>{info}<br>{seasons}";

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
