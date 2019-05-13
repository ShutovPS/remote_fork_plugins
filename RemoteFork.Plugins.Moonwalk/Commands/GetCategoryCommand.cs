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

            var item = new Item() {
                Type = ItemType.DIRECTORY,
                ImageLink = PluginSettings.Settings.Icons.IcoFolder,
                Name = film.GetTitle(),
                Link =
                    $"{GetFilmCommand.KEY}{PluginSettings.Settings.Separator}translations{PluginSettings.Settings.Separator}{WebUtility.UrlEncode(film.IframeUrl)}",
                Description = film.ToString()
            };

            return item;
        }
    }
}
